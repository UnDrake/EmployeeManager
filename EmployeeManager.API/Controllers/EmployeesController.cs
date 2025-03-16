using Microsoft.AspNetCore.Mvc;
using EmployeeManager.Shared.DTOs.Employee;
using EmployeeManager.Core.Interfaces;

namespace EmployeeManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeReadDto>>> GetEmployees([FromQuery] string? company = null)
        {
            if (company == null)
            {
                return BadRequest(new { message = "Company name is required." });
            }

            var employees = await _employeeService.GetEmployeesByCompanyAsync(company);
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeCreateDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeId = await _employeeService.AddEmployeeAsync(employeeDto);
            return CreatedAtAction(nameof(GetEmployees), new { company = employeeDto.Company },
                new { message = "Employee added successfully.", employeeId });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeUpdateDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _employeeService.UpdateEmployeeAsync(employeeDto);
            if (!updated)
            {
                return NotFound(new { message = "Employee not found." });
            }

            return Ok(new { message = "Employee updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var deleted = await _employeeService.DeleteEmployeeAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = "Employee not found." });
            }

            return Ok(new { message = "Employee deleted successfully." });
        }
    }
}
