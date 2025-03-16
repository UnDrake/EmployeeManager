using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using EmployeeManager.Desktop.Services;
using EmployeeManager.Desktop.Utils;
using EmployeeManager.Desktop.ViewModels;
using EmployeeManager.Desktop.Views;
using EmployeeManager.Shared.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManager.Desktop.ViewModels
{
    public class EmployeeListViewModel : ReactiveObject
    {
        private readonly EmployeeApiService _employeeApiService;
        private List<Employee> _allEmployees = new();
        private string _companyName = string.Empty;
        public string CompanyName
        {
            get => _companyName;
            set => this.RaiseAndSetIfChanged(ref _companyName, value);
        }

        public ObservableCollection<Employee> Employees { get; } = new();
        public ObservableCollection<string> Positions { get; } = new();
        public ObservableCollection<string> Departments { get; } = new();

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

        private string _minSalaryText = string.Empty;
        public string MinSalaryText
        {
            get => _minSalaryText;
            set
            {
                this.RaiseAndSetIfChanged(ref _minSalaryText, value);
                MinSalary = decimal.TryParse(value, out var result) ? result : null;
                ApplyFilters();
            }
        }

        private string _maxSalaryText = string.Empty;
        public string MaxSalaryText
        {
            get => _maxSalaryText;
            set
            {
                this.RaiseAndSetIfChanged(ref _maxSalaryText, value);
                MaxSalary = decimal.TryParse(value, out var result) ? result : null;
                ApplyFilters();
            }
        }

        private decimal? _minSalary;
        public decimal? MinSalary
        {
            get => _minSalary;
            private set => this.RaiseAndSetIfChanged(ref _minSalary, value);
        }

        private decimal? _maxSalary;
        public decimal? MaxSalary
        {
            get => _maxSalary;
            private set => this.RaiseAndSetIfChanged(ref _maxSalary, value);
        }

        private string _searchPhone = string.Empty;
        public string SearchPhone
        {
            get => _searchPhone;
            set
            {
                this.RaiseAndSetIfChanged(ref _searchPhone, value);
                ApplyFilters();
            }
        }

        private string _searchAddress = string.Empty;
        public string SearchAddress
        {
            get => _searchAddress;
            set
            {
                this.RaiseAndSetIfChanged(ref _searchAddress, value);
                ApplyFilters();
            }
        }

        public ICommand ApplyFiltersCommand { get; }
        public ICommand AddEmployeeCommand { get; }
        public ICommand UpdateEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ResetFiltersCommand { get; }
        public ICommand OpenSalaryReportWindowCommand { get; }

        private void OpenSalaryReportWindow()
        {
            var salaryReportWindow = new SalaryReportWindow
            {
                DataContext = new SalaryReportViewModel(Employees)
            };
            salaryReportWindow.Show();
        }

        public EmployeeListViewModel() : this("") { }

        public EmployeeListViewModel(string companyName)
        {
            _companyName = companyName;
            _employeeApiService = App.Services.GetRequiredService<EmployeeApiService>();

            ApplyFiltersCommand = ReactiveCommand.Create(ApplyFilters);
            AddEmployeeCommand = ReactiveCommand.CreateFromTask(AddEmployeeAsync);
            UpdateEmployeeCommand = ReactiveCommand.CreateFromTask(UpdateEmployeeAsync);
            DeleteEmployeeCommand = ReactiveCommand.CreateFromTask(DeleteEmployeeAsync);
            RefreshCommand = ReactiveCommand.CreateFromTask(LoadEmployeesAsync);
            ResetFiltersCommand = ReactiveCommand.Create(ResetFilters);
            OpenSalaryReportWindowCommand = ReactiveCommand.Create(OpenSalaryReportWindow);

            _ = LoadEmployeesAsync();
        }

        private async Task LoadEmployeesAsync()
        {
            var employeeDtos = await _employeeApiService.GetEmployeesByCompanyAsync(_companyName);
            _allEmployees = employeeDtos.Select(EmployeeMapper.FromReadDto).ToList();

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
            if (_allEmployees.Count == 0)
                return;

            var filtered = _allEmployees.Where(e =>
                (string.IsNullOrEmpty(SearchName) || e.FullName.Contains(SearchName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(SelectedPosition) || e.Position == SelectedPosition) &&
                (string.IsNullOrEmpty(SelectedDepartment) || e.Department == SelectedDepartment) &&
                (!MinSalary.HasValue || e.Salary >= MinSalary) &&
                (!MaxSalary.HasValue || e.Salary <= MaxSalary) &&
                (string.IsNullOrEmpty(SearchPhone) || e.Phone.Contains(SearchPhone)) &&
                (string.IsNullOrEmpty(SearchAddress) || e.Address.Contains(SearchAddress)))
                .ToList();

            Employees.Clear();
            foreach (var emp in filtered)
                Employees.Add(emp);
        }

        private void ResetFilters()
        {
            SearchName = string.Empty;
            SelectedPosition = string.Empty;
            SelectedDepartment = string.Empty;
            MinSalaryText = string.Empty;
            MaxSalaryText = string.Empty;

            ApplyFilters();
        }

        private async Task AddEmployeeAsync()
        {
            var newEmployee = new Employee
            {
                Company = _companyName
            };

            await OpenEmployeeDialog(newEmployee);
        }

        private async Task UpdateEmployeeAsync()
        {
            if (SelectedEmployee != null)
                await OpenEmployeeDialog(SelectedEmployee);
        }

        private async Task DeleteEmployeeAsync()
        {
            if (SelectedEmployee == null)
                return;

            var isDeleted = await _employeeApiService.DeleteEmployeeAsync(SelectedEmployee.ID);

            if (isDeleted)
            {
                _allEmployees.Remove(SelectedEmployee);
                Employees.Remove(SelectedEmployee);
                ApplyFilters();
            }
        }

        private async Task OpenEmployeeDialog(Employee employee)
        {
            var dialog = new Window();
            var viewModel = new EmployeeDetailViewModel(_employeeApiService, employee, dialog.Close);
            dialog.Content = new EmployeeDetailView { DataContext = viewModel };
            dialog.Width = 500;
            dialog.Height = 720;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow != null)
                await dialog.ShowDialog(desktop.MainWindow);

            await LoadEmployeesAsync();
        }
    }
}
