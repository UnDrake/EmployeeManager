using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeManager.Desktop.Services;
using EmployeeManager.Desktop.Views;
using EmployeeManager.Models;
using ReactiveUI;

namespace EmployeeManager.Desktop.ViewModels
{
    public class CompanySelectionViewModel : ReactiveObject
    {
        private readonly ApiService _apiService;
        public ObservableCollection<Company> Companies { get; } = new();

        private Company? _selectedCompany;
        public Company? SelectedCompany
        {
            get => _selectedCompany;
            set => this.RaiseAndSetIfChanged(ref _selectedCompany, value);
        }

        public ICommand SelectCompanyCommand { get; }
        public ICommand RefreshCompaniesCommand { get; }

        public CompanySelectionViewModel()
        {
            _apiService = new ApiService();
            SelectCompanyCommand = ReactiveCommand.Create(SelectCompany);
            RefreshCompaniesCommand = ReactiveCommand.CreateFromTask(LoadCompaniesAsync);

            _ = LoadCompaniesAsync();
        }

        private async Task LoadCompaniesAsync()
        {
            var companies = await _apiService.GetCompaniesAsync();
            Companies.Clear();
            foreach (var company in companies)
                Companies.Add(company);
        }
        private void SelectCompany()
        {
            if (SelectedCompany != null)
            {
                // Создаем представление (UserControl), а не модель
                var employeeListView = new EmployeeListView
                {
                    DataContext = new EmployeeListViewModel(_apiService, SelectedCompany.Name) // Привязываем модель данных
                };
                MainWindowViewModel.Instance.CurrentView = employeeListView; // Обновляем представление в главном окне
            }
        }


    }
}
