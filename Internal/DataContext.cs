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

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.HasOne(u => u.Subscription)
                    .WithMany(s => s.Users)
                    .HasForeignKey(u => u.SubscriptionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Cui);
                entity.HasIndex(u => u.SubscriptionId);
                entity.HasIndex(u => u.RoleId);
            });

            modelBuilder.Entity<Use>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.HasOne(u => u.User)
                    .WithMany(u => u.Uses)
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(u => u.IdentityCard)
                    .WithMany(i => i.Uses)
                    .HasForeignKey(u => u.IdentityCardId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(u => u.UserId);
                entity.HasIndex(u => u.IdentityCardId);
                entity.HasIndex(u => u.CreatedAt);
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.HasIndex(s => s.Name).IsUnique();
                entity.HasIndex(s => s.Price);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasIndex(r => r.Name).IsUnique();
            });

            modelBuilder.Entity<IdentityCard>(entity =>
            {
                entity.HasKey(i => i.Id);

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
