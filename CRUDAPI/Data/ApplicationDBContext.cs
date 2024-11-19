using CRUDAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }  
        public DbSet<Department> Departments { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Department>().HasIndex(d => d.Name).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(d => d.Name).IsUnique();
        }
    }
}
