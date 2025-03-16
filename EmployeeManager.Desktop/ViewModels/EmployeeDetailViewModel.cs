using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeManager.Desktop.Services;
using EmployeeManager.Shared.DTOs.Employee;
using EmployeeManager.Shared.Models;
using ReactiveUI;

namespace EmployeeManager.Desktop.ViewModels
{
    public class EmployeeDetailViewModel : ReactiveObject
    {
        private readonly EmployeeApiService _employeeApiService;
        private readonly Action _onClose;
        private Employee _employee;

        public Employee Employee
        {
            get => _employee;
            set => this.RaiseAndSetIfChanged(ref _employee, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EmployeeDetailViewModel(EmployeeApiService employeeApiService, Employee employee, Action onClose)
        {
            _employeeApiService = employeeApiService;
            _employee = employee;
            _onClose = onClose;

            SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
            CancelCommand = ReactiveCommand.Create(CloseDialog);
        }

        private async Task SaveAsync()
        {
            if (_employee.ID == 0)
            {
                var employeeDto = new EmployeeCreateDto
                {
                    FullName = _employee.FullName,
                    Phone = _employee.Phone,
                    Salary = _employee.Salary,
                    Position = _employee.Position,
                    Department = _employee.Department,
                    Address = _employee.Address,
                    Company = _employee.Company
                };

                await _employeeApiService.AddEmployeeAsync(employeeDto);
            }
            else
            {
                var employeeUpdateDto = new EmployeeUpdateDto
                {
                    ID = _employee.ID,
                    FullName = _employee.FullName,
                    Phone = _employee.Phone,
                    Salary = _employee.Salary,
                    Position = _employee.Position,
                    Department = _employee.Department,
                    Address = _employee.Address,
                    Company = _employee.Company
                };

                await _employeeApiService.UpdateEmployeeAsync(employeeUpdateDto);
            }

            CloseDialog();
        }


        private void CloseDialog() => _onClose?.Invoke();
    }
}
