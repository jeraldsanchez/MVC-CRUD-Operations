using ASPNetMVC.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ASPNetMVC.Add
{
    public class MVCDemoDbContext : DbContext
    {
        public MVCDemoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
