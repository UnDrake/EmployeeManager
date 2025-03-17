using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using EmployeeManager.Shared.Models;

namespace EmployeeManager.Desktop.ViewModels
{
    public class TotalSalaryReportByFilterViewModel : ReactiveObject
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

        public TotalSalaryReportByFilterViewModel() : this(new List<Employee>()) { }

        public TotalSalaryReportByFilterViewModel(IEnumerable<Employee> employees)
        {
            _employees = new ObservableCollection<Employee>(employees);
        }
    }
}
