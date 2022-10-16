using Microsoft.EntityFrameworkCore;
using Sny.DB.Entities;


namespace Sny.DB.Data
{
    public class SnySQLiteContext : DbContext
    {
        public SnySQLiteContext(DbContextOptions<SnySQLiteContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=..//LocalDatabase.db", b => b.MigrationsAssembly("Sny.DB"));
            }
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Entities.Task> Tasks { get; set; }
    }
}
