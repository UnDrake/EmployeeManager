using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using EmployeeManager.Desktop.Views;
using ReactiveUI;
using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using EmployeeManager.Desktop.Utils;
using Microsoft.Extensions.DependencyInjection;
using EmployeeManager.Shared.ModelsToSave;
using System.Threading.Tasks;

namespace EmployeeManager.Desktop.ViewModels
{
    public class PayrollEmployeeListViewModel : EmployeeListViewModel
    {
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

        public ICommand OpenSalaryReportWindowCommand { get; }
        public ICommand BackToEmployeeListCommand { get; }

        public PayrollEmployeeListViewModel() : this("") { }

        public PayrollEmployeeListViewModel(string companyName) : base(companyName)
        {
            OpenSalaryReportWindowCommand = ReactiveCommand.Create(OpenSalaryReportWindow);
            BackToEmployeeListCommand = ReactiveCommand.Create(BackToEmployeeList);
        }

        protected override void ApplyFilters()
        {
            if (_allEmployees.Count == 0)
                return;

            var filtered = GetFilteredEmployees().Where(e =>
                (!MinSalary.HasValue || e.Salary >= MinSalary) &&
                (!MaxSalary.HasValue || e.Salary <= MaxSalary));

            UpdateEmloyeeListByFilters(filtered);
        }

        protected override void ResetFilters()
        {
            SearchName = string.Empty;
            SelectedPosition = string.Empty;
            SelectedDepartment = string.Empty;
            MinSalaryText = string.Empty;
            MaxSalaryText = string.Empty;

            ApplyFilters();
        }

        private void OpenSalaryReportWindow()
        {
            var salaryReportWindow = new SalaryReportWindow
            {
                DataContext = new TotalSalaryReportByFilterViewModel(Employees)
            };
            salaryReportWindow.Show();
        }

        protected override async Task SaveReport()
        {
            string filters = string.Join("_", new[]
{
                string.IsNullOrEmpty(CompanyName) ? null : $"Company-{CompanyName}",
                string.IsNullOrEmpty(SearchName) ? null : $"Name-{SearchName}",
                string.IsNullOrEmpty(SelectedPosition) ? null : $"Position-{SelectedPosition}",
                string.IsNullOrEmpty(SelectedDepartment) ? null : $"Department-{SelectedDepartment}",
                string.IsNullOrEmpty(MinSalary.ToString()) ? null : $"Phone-{SearchPhone}",
                string.IsNullOrEmpty(MaxSalary.ToString()) ? null : $"Address-{SearchAddress}"
            }.Where(f => f != null));

            string reportName = string.IsNullOrEmpty(filters) ? "EmployeePayrollList.txt" : $"EmployeePayrollList_{filters}.txt";

            await _reportSaver.SaveReportAsync(_allEmployees
                .Select(EmployeeMapper.FromEmployeeToEmployeePayrollElement)
                .Cast<EmployeeBaseElement>()
                .ToList(), reportName);
        }

        private void BackToEmployeeList()
        {
            var employeeListView = new EmployeeListView
            {
                DataContext = new EmployeeListViewModel(_companyName)
            };

            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
                desktop.MainWindow is Window mainWindow)
            {
                mainWindow.Width = 880;
                mainWindow.Height = 630;
                MainWindowViewModel.Instance.CurrentView = employeeListView;
            }
        }
    }
}
