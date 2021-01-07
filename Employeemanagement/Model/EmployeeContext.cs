using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employeemanagement.Model
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<CityMaster> CityMaster { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CityMaster>().HasData(
                                   new CityMaster { Id = 1, Name = "Bangalore" },
                                   new CityMaster { Id = 2, Name = "Mysore" },
                                   new CityMaster { Id = 3, Name = "Hyderabad" },
                                   new CityMaster { Id = 4, Name = "Vijayawada" }
                                   );
        }
    }
}
