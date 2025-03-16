using EmployeeManager.Shared.DTOs.Employee;
using EmployeeManager.Shared.Models;

namespace EmployeeManager.Desktop.Utils
{
    public static class EmployeeMapper
    {
        public static EmployeeCreateDto ToCreateDto(Employee employee) => new EmployeeCreateDto
        {
            FullName = employee.FullName,
            Phone = employee.Phone,
            BirthDate = employee.BirthDate,
            HireDate = employee.HireDate,
            Salary = employee.Salary,
            Position = employee.Position,
            Department = employee.Department,
            Address = employee.Address,
            Company = employee.Company
        };

        public static EmployeeUpdateDto ToUpdateDto(Employee employee) => new EmployeeUpdateDto
        {
            ID = employee.ID,
            FullName = employee.FullName,
            Phone = employee.Phone,
            BirthDate = employee.BirthDate,
            HireDate = employee.HireDate,
            Salary = employee.Salary,
            Position = employee.Position,
            Department = employee.Department,
            Address = employee.Address,
            Company = employee.Company
        };

        public static Employee FromReadDto(EmployeeReadDto dto) => new Employee
        {
            ID = dto.ID,
            FullName = dto.FullName,
            Phone = dto.Phone,
            BirthDate = dto.BirthDate,
            HireDate = dto.HireDate,
            Salary = dto.Salary,
            Position = dto.Position,
            Department = dto.Department,
            Address = dto.Address,
            Company = dto.Company
        };
    }
}
