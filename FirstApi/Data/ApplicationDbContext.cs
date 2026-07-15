using FirstApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User> AccountUsers { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
