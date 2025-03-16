using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using EmployeeManager.Shared.Models;

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
                this.RaisePropertyChanged(nameof(TotalSalary));
            }
        }

        public decimal TotalSalary => Employees.Sum(e => e.Salary);

        public ICommand SaveReportCommand { get; }

        public SalaryReportViewModel() : this(new List<Employee>()) { }

        public SalaryReportViewModel(IEnumerable<Employee> employees)
        {
            _employees = new ObservableCollection<Employee>(employees);
            SaveReportCommand = ReactiveCommand.Create(SaveReport);
        }

        private void SaveReport()
        {
            const string filePath = "salary_report.txt";
            using (var writer = new StreamWriter(filePath))
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
