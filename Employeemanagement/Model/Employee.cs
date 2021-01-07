using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Employeemanagement.Model
{
    public class ApplicationUser : IdentityUser
    {
    }
   
    
    
    public class Employee
    {
        public Employee()
        {
            Datecreated = DateTime.UtcNow;
        }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required.")]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Employee Designation is required.")]
        [Column(TypeName = "int")]
        [ForeignKey("CityMaster")]
        public int CityId { get; set; }
        public  CityMaster CityMaster { get; set; }
        [Required(ErrorMessage = "Phone Number is required.")]
        [Column(TypeName = "nvarchar(10)")]
        public string PhoneNumber { get; set; }
    
        public DateTime Datecreated { get; set; }
      

    }


    public class CityMaster
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class EmployeeData
    {
        public List<Employee> employeelst { get; set; }
    }
 
}
