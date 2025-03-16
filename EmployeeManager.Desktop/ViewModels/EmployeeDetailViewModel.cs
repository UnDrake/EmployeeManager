using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeManager.Desktop.Services;
using EmployeeManager.Models;
using ReactiveUI;

namespace EmployeeManager.Desktop.ViewModels
{
    public class EmployeeDetailViewModel : ReactiveObject
    {
        private readonly ApiService _apiService;
        private readonly Action _onClose;
        private Employee _employee;

        public Employee Employee
        {
            get => _employee;
            set => this.RaiseAndSetIfChanged(ref _employee, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EmployeeDetailViewModel(ApiService apiService, Employee employee, Action onClose)
        {
            _apiService = apiService;
            _employee = employee;
            _onClose = onClose;

            SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
            CancelCommand = ReactiveCommand.Create(CloseDialog);
        }

        private async Task SaveAsync()
        {
            if (_employee.ID == 0)
                await _apiService.AddEmployeeAsync(_employee);
            else
                await _apiService.UpdateEmployeeAsync(_employee);

            CloseDialog();
        }

        private void CloseDialog() => _onClose?.Invoke();
    }
}
