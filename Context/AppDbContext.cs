using ExtractInfoIdentityDocument.Models;

using Microsoft.EntityFrameworkCore;

namespace ExtractInfoIdentityDocument.Context
{
    // Folosim Configuration

    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Default");
        }
    }
}
