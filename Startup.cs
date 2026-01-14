
using Data.SDK;
using Data.SDK.Repository;
using Data.SDK.Repository.Interface;

using ExtractInfoIdentityDocument.Internal;
using ExtractInfoIdentityDocument.Services;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.AspNetCore.Authentication.JwtBearer; // Necesare pentru JWT
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens; // Necesare pentru validarea token-ului
using Microsoft.OpenApi.Models;

using System; // Adaugat System pentru diverse tipuri
using System.Collections.Generic;
using System.Security.Principal;
using System.Text; // Pentru Encoding

namespace ExtractInfoIdentityDocument
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // --- 1. Configurare Swagger (deja existentă, bună pentru JWT) ---
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExtractInfoIdentityDocument", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",

                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                  {
                    new OpenApiSecurityScheme
                    {
                      Reference = new OpenApiReference
                        {
                          Type = ReferenceType.SecurityScheme,
                          Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                      },
                      new List<string>()
                    }
                  });
            });

            // --- 2. Configurare Database Context ---
            services.AddDbContext<DataContext>(context =>
            {
                context.UseSqlServer(Configuration.GetConnectionString("Default"));

                context.EnableSensitiveDataLogging();

                context.LogTo(Console.WriteLine);
            });

            // --- 3. Configurare Autentificare JWT (NOU) ---
            // Asigură-te că ai secțiunea "JwtSettings" în appsettings.json
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:Key"])),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Opțional: elimină toleranța implicită de 5 min
                };
            });

            // --- 4. Înregistrare Servicii ---
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);

            services.AddControllersWithViews(); // MVC

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Serviciile existente
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUseService, UseService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IIdentityCardService, IdentityCardService>();
            services.AddScoped<IIdentityDocumentAnalyzerService, IdentityDocumentAnalyzerService>();

            // Serviciile NOI pentru Auth
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserContextService, UserContextService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExtractInfoIdentityDocument v1");
                c.RoutePrefix = "";
            });
            var option = new RewriteOptions();
            option.AddRedirect("^$", "index.html");
            app.UseRewriter(option);
            app.UseHttpsRedirection();

            app.UseRouting();

            // --- 5. Activare Middleware Autentificare (NOU) ---
            // Ordinea este critică: UseAuthentication înainte de UseAuthorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "{controller=Admin}/{action=Index}/{id?}"
                );

                endpoints.MapControllers();
            });
        }
    }
}
