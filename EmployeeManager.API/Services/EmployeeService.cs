using EmployeeManager.API.DTOs;
using EmployeeManager.API.Interfaces;
using EmployeeManager.Data.Interfaces;
using EmployeeManager.Data.Repositories;
using EmployeeManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeService(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // ✅ Получение всех сотрудников и конвертация в DTO
        public List<EmployeeReadDto> GetAllEmployees()
        {
            return _employeeRepository.GetAll()
                .Select(e => new EmployeeReadDto
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
                })
                .ToList();
        }

        // ✅ Добавление нового сотрудника (используем DTO)
        public async Task AddEmployee(EmployeeCreateDto employeeDto)
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

            await _employeeRepository.Create(employee);
        }

        // ✅ Обновление сотрудника
        public async Task UpdateEmployee(Employee employee)
        {
            await _employeeRepository.Update(employee);
        }

        // ✅ Удаление сотрудника
        public async Task DeleteEmployee(int id)
        {
            var employee = new Employee { ID = id };
            await _employeeRepository.Delete(employee);
        }

        public List<EmployeeReadDto> GetEmployeesByCompany(string companyName)
        {
            var employees = _employeeRepository.GetEmployeesByCompany(companyName);
            return employees.Select(e => new EmployeeReadDto(e)).ToList();
        }

    }
}
