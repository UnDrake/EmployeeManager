using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EmployeeManager.Shared.Models;
using System.Collections.Generic;

namespace EmployeeManager.Desktop.ViewModels
{
    public class SalaryReportViewModel : ReactiveObject
    {
        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                this.RaiseAndSetIfChanged(ref _employees, value);
                this.RaisePropertyChanged(nameof(TotalSalary));  // Notify that total salary changed
            }
        }

        // Property to hold departments
        private ObservableCollection<string> _departments;
        public ObservableCollection<string> Departments
        {
            get => _departments;
            set => this.RaiseAndSetIfChanged(ref _departments, value);
        }

        // Total salary for the filtered employees
        public decimal TotalSalary => Employees?.Sum(e => e.Salary) ?? 0;

        // Department filter (for generating report)
        private string _selectedDepartment;
        public string SelectedDepartment
        {
            get => _selectedDepartment;
            set => this.RaiseAndSetIfChanged(ref _selectedDepartment, value);
        }

        // Command to save the report
        public ICommand SaveReportCommand { get; }

        // Command to generate the report
        public ICommand GenerateReportCommand { get; }

        // Constructor
        public SalaryReportViewModel()
        {
            SaveReportCommand = ReactiveCommand.Create(SaveReport);
            GenerateReportCommand = ReactiveCommand.Create(GenerateReport);
        }

        // Constructor with filtered employees
        public SalaryReportViewModel(IEnumerable<Employee> employees) : this()
        {
            Employees = new ObservableCollection<Employee>(employees);
            Departments = new ObservableCollection<string>(employees.Select(e => e.Department).Distinct());
        }

        // Logic to generate report, typically you'd do calculations or transformations here
        private void GenerateReport()
        {
            // Logic to generate report based on selected department or other filters
            // This is just an example, customize as needed.
            var filteredEmployees = Employees.Where(e => e.Department == SelectedDepartment).ToList();
            Employees = new ObservableCollection<Employee>(filteredEmployees);
        }

        // Logic to save the report in a file
        private void SaveReport()
        {
            var filePath = "salary_report.txt"; // File path to save the report
            using (var writer = new System.IO.StreamWriter(filePath))
            {
                writer.WriteLine("Salary Report");
                writer.WriteLine("=======================================");
                foreach (var employee in Employees)
                {
                    writer.WriteLine($"{employee.FullName} - {employee.Salary}");
                }
            }
        }
    }
}
