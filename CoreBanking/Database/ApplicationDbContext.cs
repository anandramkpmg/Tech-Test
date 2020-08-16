using CoreBanking.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreBanking.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
