using Data.SDK.Repository.Interface;

using ExtractInfoIdentityDocument.Models;

using Microsoft.EntityFrameworkCore;

namespace ExtractInfoIdentityDocument.Internal
{
    public class DataContext : DbContext, IDbContext
    {
        public DataContext(DbContextOptions options)
          : base(options)
        { }

        public void DetachAllEntities()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === User ===
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Email)
                    .HasMaxLength(255);

                entity.Property(u => u.Email)
                    .HasMaxLength(255);

                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Cui);
                entity.HasIndex(u => u.SubscriptionId);
                entity.HasIndex(u => u.RoleId);

                // Relații fără inverse
                entity.HasOne(u => u.Subscription)
                    .WithMany() // fără colecție inversă
                    .HasForeignKey(u => u.SubscriptionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.Role)
                    .WithMany() // fără colecție inversă
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // === Use ===
            modelBuilder.Entity<Use>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(u => u.ModifiedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(u => u.UserId);
                entity.HasIndex(u => u.IdentityCardId);
                entity.HasIndex(u => u.CreatedAt);

                entity.HasOne(u => u.User)
                    .WithMany() // fără colecție inversă
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(u => u.IdentityCard)
                    .WithMany() // fără colecție inversă
                    .HasForeignKey(u => u.IdentityCardId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // === Subscription ===
            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Name)
                    .HasMaxLength(100);

                entity.Property(s => s.Price)
                    .HasPrecision(10, 2);

                entity.HasIndex(s => s.Name).IsUnique();

                entity.HasIndex(s => s.Price);

                entity.HasIndex(s => s.Name)
                    .IsUnique()
                    .HasDatabaseName("IX_Subscription_Name");

                entity.Property(s => s.IsDefault)
                    .HasDefaultValue(false);

                // 👇 Filtrare: doar un singur rând poate avea IsDefault = true
                entity.HasIndex(s => s.IsDefault)
                    .IsUnique()
                    .HasFilter("[IsDefault] = 1")
                    .HasDatabaseName("IX_Subscription_IsDefault_True");
            });

            // === Role ===
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");

                entity.HasKey(r => r.Id);

                entity.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(r => r.Name)
                    .IsUnique()
                    .HasDatabaseName("IX_Role_Name");

                entity.Property(r => r.IsDefault)
                    .HasDefaultValue(false);

                // 👇 Filtrare: doar un singur rând poate avea IsDefault = true
                entity.HasIndex(r => r.IsDefault)
                    .IsUnique()
                    .HasFilter("[IsDefault] = 1")
                    .HasDatabaseName("IX_Role_IsDefault_True");
            });


            // === IdentityCard ===
            modelBuilder.Entity<IdentityCard>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(i => i.Cnp)
                    .HasMaxLength(13);

                entity.Property(i => i.Series)
                    .HasMaxLength(20);

                entity.HasIndex(i => i.Cnp).IsUnique();
                entity.HasIndex(i => i.Series);
                entity.HasIndex(i => new { i.LastName, i.FirstName });
                entity.HasIndex(i => i.City);
            });
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<IdentityCard> IdentityCards { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Use> Uses { get; set; }

        public async Task<int> ExecuteSqlRawAsync(string sql, int timeout = 0,
             params object[] parameters)
        {
            if (timeout > 0)
            {
                Database.SetCommandTimeout(timeout);
            }

            var result = await Database.ExecuteSqlRawAsync(sql, parameters);
            Database.SetCommandTimeout(null);
            return result;
        }
    }
}
