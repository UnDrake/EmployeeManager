using Avalonia.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeManager.Desktop.Services;
using EmployeeManager.Models;
using ReactiveUI;
using EmployeeManager.Desktop.Views;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManager.Desktop.ViewModels
{
    public class EmployeeListViewModel : ReactiveObject
    {
        private readonly ApiService _apiService;
        private List<Employee> _allEmployees = new();

        public ObservableCollection<Employee> Employees { get; } = new();

        private Employee? _selectedEmployee;
        public Employee? SelectedEmployee
        {
            get => _selectedEmployee;
            set => this.RaiseAndSetIfChanged(ref _selectedEmployee, value);
        }

        private string _searchName = string.Empty;
        public string SearchName
        {
            get => _searchName;
            set
            {
                this.RaiseAndSetIfChanged(ref _searchName, value);
                ApplyFilters();
            }
        }

        private string _selectedPosition = string.Empty;
        public string SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedPosition, value);
                ApplyFilters();
            }
        }

        private string _selectedDepartment = string.Empty;
        public string SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedDepartment, value);
                ApplyFilters();
            }
        }

        public ObservableCollection<string> Positions { get; } = new();
        public ObservableCollection<string> Departments { get; } = new();

        public ICommand ApplyFiltersCommand { get; }
        public ICommand AddEmployeeCommand { get; }
        public ICommand UpdateEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand RefreshCommand { get; }

        public EmployeeListViewModel()
        {
            _apiService = new ApiService();

            ApplyFiltersCommand = ReactiveCommand.Create(ApplyFilters);
            AddEmployeeCommand = ReactiveCommand.CreateFromTask(AddEmployeeAsync);
            UpdateEmployeeCommand = ReactiveCommand.CreateFromTask(UpdateEmployeeAsync);
            DeleteEmployeeCommand = ReactiveCommand.CreateFromTask(DeleteEmployeeAsync);
            RefreshCommand = ReactiveCommand.CreateFromTask(LoadEmployeesAsync);

            _ = LoadEmployeesAsync();
        }

        private async Task LoadEmployeesAsync()
        {
            _allEmployees = await _apiService.GetEmployeesAsync();
            Employees.Clear();
            Positions.Clear();
            Departments.Clear();

            foreach (var emp in _allEmployees)
            {
                Employees.Add(emp);

                if (!Positions.Contains(emp.Position)) Positions.Add(emp.Position);
                if (!Departments.Contains(emp.Department)) Departments.Add(emp.Department);
            }
        }

        private void ApplyFilters()
        {
            if (_allEmployees == null || _allEmployees.Count == 0)
                return;

            var filtered = _allEmployees.Where(e =>
                (string.IsNullOrEmpty(SearchName) || e.FullName.Contains(SearchName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(SelectedPosition) || e.Position == SelectedPosition) &&
                (string.IsNullOrEmpty(SelectedDepartment) || e.Department == SelectedDepartment)
            ).ToList();

            Employees.Clear();
            foreach (var emp in filtered)
                Employees.Add(emp);
        }

        private async Task AddEmployeeAsync()
        {
            var newEmployee = new Employee
            {
                FullName = "Новый сотрудник",
                Phone = "1234567890",
                Position = "Разработчик",
                BirthDate = DateTime.Now.AddYears(-25),
                HireDate = DateTime.Now,
                Salary = 50000,
                Department = "QA",
                Address = "Примерный адрес"
            };

            await OpenEmployeeDialog(newEmployee);
        }

        private async Task UpdateEmployeeAsync()
        {
            if (SelectedEmployee != null)
            {
                await OpenEmployeeDialog(SelectedEmployee);
            }
        }

        private async Task DeleteEmployeeAsync()
        {
            if (SelectedEmployee != null && await _apiService.DeleteEmployeeAsync(SelectedEmployee.ID))
                Employees.Remove(SelectedEmployee);
        }

        private async Task OpenEmployeeDialog(Employee employee)
        {
            var dialog = new Window();
            var viewModel = new EmployeeDetailViewModel(_apiService, employee, dialog.Close);

            dialog.Content = new EmployeeDetailView { DataContext = viewModel };
            dialog.Width = 500;
            dialog.Height = 450;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                await dialog.ShowDialog(desktop.MainWindow);
            }

            await LoadEmployeesAsync();
        }
    }
}