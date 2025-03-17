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
using EmployeeManager.Shared.ModelsToSave;

namespace EmployeeManager.Desktop.ViewModels
{
    public class EmployeeListViewModel : ReactiveObject
    {
        private readonly EmployeeApiService _employeeApiService;
        protected List<Employee> _allEmployees = new();
        protected string _companyName = string.Empty;
        protected readonly ReportSaver _reportSaver;

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
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedEmployee, value);
                this.RaisePropertyChanged(nameof(IsEmployeeSelected));
            }
        }

        public bool IsEmployeeSelected => SelectedEmployee != null;


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
        public ICommand OpenPayrollEmployeeListCommand { get; }
        public ICommand BackToCompanySelectionCommand { get; }
        public ICommand SaveReportCommand { get; }

        public EmployeeListViewModel() : this("") { }

        public EmployeeListViewModel(string companyName)
        {
            _companyName = companyName;
            _employeeApiService = App.Services.GetRequiredService<EmployeeApiService>();
            _reportSaver = App.Services.GetRequiredService<ReportSaver>();

            ApplyFiltersCommand = ReactiveCommand.Create(ApplyFilters);
            AddEmployeeCommand = ReactiveCommand.CreateFromTask(AddEmployeeAsync);
            UpdateEmployeeCommand = ReactiveCommand.CreateFromTask(UpdateEmployeeAsync, this.WhenAnyValue(x => x.IsEmployeeSelected));
            DeleteEmployeeCommand = ReactiveCommand.CreateFromTask(DeleteEmployeeAsync, this.WhenAnyValue(x => x.IsEmployeeSelected));
            RefreshCommand = ReactiveCommand.CreateFromTask(LoadEmployeesAsync);
            ResetFiltersCommand = ReactiveCommand.Create(ResetFilters);
            OpenPayrollEmployeeListCommand = ReactiveCommand.Create(OpenPayrollEmployeeList);
            BackToCompanySelectionCommand = ReactiveCommand.Create(BackToCompanySelection);
            SaveReportCommand = ReactiveCommand.Create(SaveReport);

            _ = LoadEmployeesAsync();
        }

        protected async Task LoadEmployeesAsync()
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

        protected virtual void ApplyFilters()
        {
            if (_allEmployees.Count == 0)
                return;

            var filtered = _allEmployees.Where(e =>
                (string.IsNullOrEmpty(SearchName) || e.FullName.Contains(SearchName, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(SelectedPosition) || e.Position == SelectedPosition) &&
                (string.IsNullOrEmpty(SelectedDepartment) || e.Department == SelectedDepartment) &&
                (string.IsNullOrEmpty(SearchPhone) || e.Phone.Contains(SearchPhone)) &&
                (string.IsNullOrEmpty(SearchAddress) || e.Address.Contains(SearchAddress)));

            UpdateEmloyeeListByFilters(filtered);
        }

        protected void UpdateEmloyeeListByFilters(IEnumerable<Employee> filteredEmployees)
        {
            Employees.Clear();
            foreach (var emp in filteredEmployees)
                Employees.Add(emp);
        }

        protected virtual void ResetFilters()
        {
            SearchName = string.Empty;
            SelectedPosition = string.Empty;
            SelectedDepartment = string.Empty;
            SearchPhone = string.Empty;
            SearchAddress = string.Empty;

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
            dialog.Height = 750;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow != null)
                await dialog.ShowDialog(desktop.MainWindow);

            await LoadEmployeesAsync();
        }

        private void OpenPayrollEmployeeList()
        {
            var payrollViewModel = new PayrollEmployeeListView
            {
                DataContext = new PayrollEmployeeListViewModel(_companyName)
            };

            if (App.Current != null && App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
                desktop.MainWindow is Window mainWindow)
            {
                mainWindow.Width = 880;
                mainWindow.Height = 620;
                MainWindowViewModel.Instance.CurrentView = payrollViewModel;
            }
        }

        private void BackToCompanySelection()
        {
            var companySelectionView = new CompanySelectionView
            {
                DataContext = new CompanySelectionViewModel()
            };

            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
                desktop.MainWindow is Window mainWindow)
            {
                mainWindow.Width = 550;
                mainWindow.Height = 330;
                MainWindowViewModel.Instance.CurrentView = companySelectionView;
            }
        }

        protected virtual async Task SaveReport()
        {
            string filters = string.Join("_", new[]
            {
                string.IsNullOrEmpty(CompanyName) ? null : $"Company-{CompanyName}",
                string.IsNullOrEmpty(SearchName) ? null : $"Name-{SearchName}",
                string.IsNullOrEmpty(SelectedPosition) ? null : $"Position-{SelectedPosition}",
                string.IsNullOrEmpty(SelectedDepartment) ? null : $"Department-{SelectedDepartment}",
                string.IsNullOrEmpty(SearchPhone) ? null : $"Phone-{SearchPhone}",
                string.IsNullOrEmpty(SearchAddress) ? null : $"Address-{SearchAddress}"
            }.Where(f => f != null));

            string reportName = string.IsNullOrEmpty(filters) ? "EmployeeListReport.txt" : $"EmployeeListReport_{filters}.txt";

            await _reportSaver.SaveReportAsync(_allEmployees
                .Select(EmployeeMapper.FromEmployeeToEmployeeListElement)
                .Cast<EmployeeBaseElement>()
                .ToList(), reportName);
        }
    }
}
