using ExtractInfoIdentityDocument.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExtractInfoIdentityDocument.Controllers
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscriptions");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(s => s.Name)
                   .IsUnique();

            builder.Property(s => s.Price)
                   .IsRequired()
                   .HasPrecision(10, 2);

            builder.HasCheckConstraint("CK_Subscription_Price_Positive", "[Price] > 0");
        }
    }
}
