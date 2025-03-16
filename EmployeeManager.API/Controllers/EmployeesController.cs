using EmployeeManager.API.DTOs;
using EmployeeManager.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EmployeeManager.Models;
using EmployeeManager.API.Services;

namespace EmployeeManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /*[HttpGet]
        public async Task<ActionResult<List<EmployeeReadDto>>> GetEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            return Ok(employees);
        }*/

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeCreateDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _employeeService.AddEmployee(employeeDto);
            return Ok(new { message = "Сотрудник добавлен успешно" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _employeeService.UpdateEmployee(employee);
            return Ok(new { message = "Данные сотрудника обновлены" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await _employeeService.DeleteEmployee(id);
            return Ok(new { message = "Сотрудник удалён" });
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployees([FromQuery] string? company = null)
        {
            var employees = company == null
                ? await _employeeService.GetAllEmployees()
                : await _employeeService.GetEmployeesByCompany(company);

            return Ok(employees);
        }

    }
}
