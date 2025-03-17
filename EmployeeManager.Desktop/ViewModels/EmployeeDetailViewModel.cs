using EmployeeManager.Desktop.Services;
using EmployeeManager.Desktop.Utils;
using EmployeeManager.Shared.Models;
using ReactiveUI;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

namespace EmployeeManager.Desktop.ViewModels
{

    public class EmployeeDetailViewModel : ReactiveObject
    {
        private readonly EmployeeApiService _employeeApiService;
        private readonly Action _onClose;
        private readonly Employee _employee;

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        private string _salaryText = string.Empty;
        public string SalaryText
        {
            get => _salaryText;
            set
            {
                this.RaiseAndSetIfChanged(ref _salaryText, value);
                Salary = decimal.TryParse(value, out var result) ? result : null;
            }
        }

        private decimal? _salary;
        public decimal? Salary
        {
            get => _salary;
            private set
            {
                this.RaiseAndSetIfChanged(ref _salary, value);
                _employee.Salary = Salary ?? 0m;
            }
        }

        public Employee Employee => _employee;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EmployeeDetailViewModel(EmployeeApiService employeeApiService, Employee employee, Action onClose)
        {
            _employeeApiService = employeeApiService;
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _onClose = onClose ?? throw new ArgumentNullException(nameof(onClose));

            _salaryText = _employee.Salary.ToString() ?? string.Empty;

            SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
            CancelCommand = ReactiveCommand.Create(CloseDialog);
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Employee.FullName) ||
                string.IsNullOrWhiteSpace(Employee.Phone) ||
                Employee.BirthDate == null ||
                Employee.HireDate == null ||
                string.IsNullOrWhiteSpace(Employee.Position) ||
                string.IsNullOrWhiteSpace(Employee.Department) ||
                string.IsNullOrWhiteSpace(Employee.Address))
            {
                ErrorMessage = "All fields mustn't be empty.";
                return; 
            }

            if (Employee.ID == 0)
            {
                await _employeeApiService.AddEmployeeAsync(EmployeeMapper.ToCreateDto(_employee));
            }
            else
            {
                await _employeeApiService.UpdateEmployeeAsync(EmployeeMapper.ToUpdateDto(_employee));
            }

            CloseDialog();
        }

        private void CloseDialog() => _onClose.Invoke();
    }

}
