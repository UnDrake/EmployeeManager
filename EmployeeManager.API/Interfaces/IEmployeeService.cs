using EmployeeManager.API.DTOs;
using EmployeeManager.Models;

namespace EmployeeManager.API.Interfaces
{
    public interface IEmployeeService
    {
        List<EmployeeReadDto> GetAllEmployees();
        Task AddEmployee(EmployeeCreateDto employeeDto);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(int id);
    }
}
