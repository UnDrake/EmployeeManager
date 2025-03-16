﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeManager.Desktop.Services;
using EmployeeManager.Desktop.Utils;
using EmployeeManager.Shared.Models;
using ReactiveUI;

namespace EmployeeManager.Desktop.ViewModels
{
    public class EmployeeDetailViewModel : ReactiveObject
    {
        private readonly EmployeeApiService _employeeApiService;
        private readonly Action _onClose;
        private readonly Employee _employee;

        public Employee Employee => _employee;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EmployeeDetailViewModel(EmployeeApiService employeeApiService, Employee employee, Action onClose)
        {
            _employeeApiService = employeeApiService;
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _onClose = onClose ?? throw new ArgumentNullException(nameof(onClose));

            SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
            CancelCommand = ReactiveCommand.Create(CloseDialog);
        }

        private async Task SaveAsync()
        {
            if (_employee.ID == 0)
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
