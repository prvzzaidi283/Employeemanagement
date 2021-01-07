using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employeemanagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Employeemanagement.Controllers
{
    [Route("employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _context;
        protected readonly ILogger Logger;
        public EmployeeController(ILogger<EmployeeController> logger, EmployeeContext context)
        {
            _context = context;
            Logger = logger;
        }
        [HttpGet]
        [Route("employeelist")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeeList()
        {
          
            //Logger?.LogDebug("'{0} has been invoked'",nameof(GetEmployeeList));
            EmployeeData vm = new EmployeeData();
            vm.employeelst = await _context.Employee.Include(x => x.CityMaster).ToListAsync();
            return vm.employeelst;
        }
        [HttpGet]
        [Route("citylist")]
        public async Task<ActionResult<IEnumerable<CityMaster>>> GetCityList()
        {
            List<CityMaster> CityList = new List<CityMaster>();
            //Logger?.LogDebug("'{0} has been invoked'",nameof(GetEmployeeList));
            CityList = await _context.CityMaster.ToListAsync();
            return CityList;
        }
        [HttpPost]
        [Route("createemployee")]
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee employee)
        {
            
            ModelState.Remove("Datecreated");
            if (ModelState.IsValid)
            {
                _context.Employee.Add(employee);
                await _context.SaveChangesAsync();
                ModelState.Clear();
                return CreatedAtAction(nameof(GetEmployeeList), new { id = employee.Id }, employee);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpGet("getemployeebyId/{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Employee>> getemployeebyId(int id)
        { 
            
           
            var employeeDetail = await _context.Employee.FindAsync(id);
            if (employeeDetail == null)
            {
                return NotFound();
            }


            return employeeDetail;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            ModelState.Remove("Datecreated");

            if (id != employee.Id)
            {
                return BadRequest();
            }
            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeedetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeletePaymentDetail(int id)
        {
            var employeeDetail = await _context.Employee.FindAsync(id);
            if (employeeDetail == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employeeDetail);
            await _context.SaveChangesAsync();

            return employeeDetail;
        }
       
        private bool EmployeedetailsExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
        
    }
}