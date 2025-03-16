using EmployeeManager.Core.Interfaces;
using EmployeeManager.Data.Repositories;
using EmployeeManager.Shared.DTOs.Employee;
using EmployeeManager.Shared.Models;

namespace EmployeeManager.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeService(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public async Task<IEnumerable<EmployeeReadDto>> GetEmployeesByCompanyAsync(string companyName)
        {
            var employees = await _employeeRepository.GetEmployeesByCompanyAsync(companyName);
            return employees.Select(e => new EmployeeReadDto
            {
                ID = e.ID,
                FullName = e.FullName,
                Phone = e.Phone,
                BirthDate = e.BirthDate,
                HireDate = e.HireDate,
                Salary = e.Salary,
                Position = e.Position,
                Department = e.Department,
                Address = e.Address,
                Company = e.Company
            });
        }

        public async Task<int> AddEmployeeAsync(EmployeeCreateDto employeeDto)
        {
            var employee = new Employee
            {
                FullName = employeeDto.FullName,
                Phone = employeeDto.Phone,
                BirthDate = employeeDto.BirthDate,
                HireDate = employeeDto.HireDate,
                Salary = employeeDto.Salary,
                Position = employeeDto.Position,
                Department = employeeDto.Department,
                Address = employeeDto.Address,
                Company = employeeDto.Company
            };

            return await _employeeRepository.CreateAsync(employee);
        }

        public async Task<bool> UpdateEmployeeAsync(EmployeeUpdateDto employeeDto)
        {
            var employee = new Employee
            {
                ID = employeeDto.ID,
                FullName = employeeDto.FullName,
                Phone = employeeDto.Phone,
                BirthDate = employeeDto.BirthDate,
                HireDate = employeeDto.HireDate,
                Salary = employeeDto.Salary,
                Position = employeeDto.Position,
                Department = employeeDto.Department,
                Address = employeeDto.Address,
                Company = employeeDto.Company
            };

            return await _employeeRepository.UpdateAsync(employee);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            return await _employeeRepository.DeleteAsync(id);
        }
    }
}
