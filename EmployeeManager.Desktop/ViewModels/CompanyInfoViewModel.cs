using EmployeeManager.Models;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace EmployeeManager.Desktop.ViewModels
{
    public class CompanyInfoViewModel : ReactiveObject
    {
        private readonly Action _onClose;

        private Company _selectedCompany;
        public Company SelectedCompany
        {
            get => _selectedCompany;
            set => this.RaiseAndSetIfChanged(ref _selectedCompany, value);
        }

        public CompanyInfoViewModel(Company company, Action onClose)
        {
            _onClose = onClose;
            _selectedCompany = company;
        }

        private void CloseDialog() => _onClose?.Invoke();
    }
}
