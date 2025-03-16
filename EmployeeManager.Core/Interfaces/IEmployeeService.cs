using EmployeeManager.Shared.DTOs.Employee;

namespace EmployeeManager.Core.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeReadDto>> GetEmployeesByCompanyAsync(string companyName);
        Task<int> AddEmployeeAsync(EmployeeCreateDto employeeDto);
        Task<bool> UpdateEmployeeAsync(EmployeeUpdateDto employeeDto);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
