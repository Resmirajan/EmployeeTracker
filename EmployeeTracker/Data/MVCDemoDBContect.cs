using EmployeeTracker.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Data
{
    public class MVCDemoDBContect : DbContext
    {
        public MVCDemoDBContect(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
