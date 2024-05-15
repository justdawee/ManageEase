using EntityFramework.Exceptions.MySQL.Pomelo;
using ManageEase.Entities;
using Microsoft.EntityFrameworkCore;

namespace ManageEase
{
    public class ManageEaseDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Employee> Employees => Set<Employee>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseExceptionProcessor() // Enables handling of database-specific exceptions (like unique constraint violations)
                .UseMySql(
                    "server=ip;database=db;user=usr;password=pass;port=3306",
                    new MariaDbServerVersion("10.6.16") // Specifies the version of the MariaDB(or MySQL) server
                );
        }
    }
}