
using Data.SDK;
using Data.SDK.Repository;
using Data.SDK.Repository.Interface;

using ExtractInfoIdentityDocument.Internal;
using ExtractInfoIdentityDocument.Services;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.AspNetCore.Authentication.Cookies;
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

        // Această metodă este apelată de runtime. Folosește-o pentru a adăuga servicii în container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200", "https://localhost:4200") // URL-ul frontend-ului tău
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials(); // Important dacă folosești cookie-uri sau auth headers
                    });
            });

            // --- 1. Configurare Swagger ---
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

                // Poți comenta aceste două linii în producție pentru performanță/securitate
                context.EnableSensitiveDataLogging();
                context.LogTo(Console.WriteLine);
            });

            // --- 3. Configurare Autentificare ---
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
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
                    ClockSkew = TimeSpan.Zero
                };
            });

            // --- 4. Înregistrare Servicii (Dependency Injection) ---
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);

            services.AddControllersWithViews(); // Necesar pentru MVC

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Serviciile aplicației
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUseService, UseService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IIdentityCardService, IdentityCardService>();
            services.AddScoped<IIdentityDocumentAnalyzerService, IdentityDocumentAnalyzerService>();

            // Servicii pentru Auth/Token
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IFileLoggingService, FileLoggingService>();
        }

        // Această metodă este apelată de runtime. Folosește-o pentru a configura pipeline-ul HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            // Activăm Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExtractInfoIdentityDocument v1");
                // MODIFICAT: Am scos RoutePrefix = "" pentru ca Swagger să fie la /swagger, nu pe root.
            });

            // MODIFICAT: Am scos blocul RewriteOptions care redirecționa root-ul la index.html (Swagger)
            // Astfel, root-ul "/" va fi preluat de ruta default MVC (Admin/Index).

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // Necesar pentru CSS/JS în Login și Admin

            app.UseRouting();

            app.UseCors("AllowFrontend");

            // --- 5. Activare Middleware Autentificare ---
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Ruta default pentru admin/MVC
                // Când accesezi "/", te va duce la AdminController -> Index (sau Login dacă nu ești autentificat)
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Admin}/{action=Index}/{id?}"
                );

                endpoints.MapControllers();
            });
        }
    }
}
