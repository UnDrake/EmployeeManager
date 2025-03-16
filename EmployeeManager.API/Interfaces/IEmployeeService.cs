using EmployeeManager.API.DTOs;
using EmployeeManager.Models;

namespace EmployeeManager.API.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeReadDto>> GetAllEmployees();
        Task AddEmployee(EmployeeCreateDto employeeDto);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(int id);
    }
}
