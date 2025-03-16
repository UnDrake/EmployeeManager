using EmployeeManager.Shared.DTOs.Company;
using EmployeeManager.Shared.Models;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace EmployeeManager.Desktop.ViewModels
{
    public class CompanyInfoViewModel : ReactiveObject
    {
        public CompanyReadDto SelectedCompany { get; }

        public CompanyInfoViewModel(CompanyReadDto company)
        {
            SelectedCompany = company;
        }
    }
}
