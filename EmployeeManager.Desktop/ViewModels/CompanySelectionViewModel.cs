using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
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

        public bool IsCompanySelected => SelectedCompany != null;


        private Company? _selectedCompany;
        public Company? SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedCompany, value);
                this.RaisePropertyChanged(nameof(IsCompanySelected)); // Уведомляем об изменении свойства для обновления кнопки
            }
        }


        public ICommand SelectCompanyCommand { get; }
        public ICommand RefreshCompaniesCommand { get; }
        public ICommand DeselectCompanyCommand { get; }


        public CompanySelectionViewModel()
        {
            _apiService = new ApiService();
            SelectCompanyCommand = ReactiveCommand.Create(SelectCompany);
            RefreshCompaniesCommand = ReactiveCommand.CreateFromTask(LoadCompaniesAsync);
            ViewCompanyInfoCommand = ReactiveCommand.Create(ViewCompanyInfo, this.WhenAnyValue(x => x.IsCompanySelected));
            DeselectCompanyCommand = ReactiveCommand.Create(DeselectCompany);

            _ = LoadCompaniesAsync();
        }

        private void DeselectCompany()
        {
            SelectedCompany = null; // Отменяем выбор компании
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

        public ICommand ViewCompanyInfoCommand { get; }

        private async Task ViewCompanyInfo()
        {
            if (SelectedCompany != null)
            {
                var dialog = new CompanyInfoWindow();
                var viewModel = new CompanyInfoViewModel(SelectedCompany, dialog.Close);

                // Устанавливаем DataContext
                dialog.DataContext = viewModel;

                // Настроим размер окна
                dialog.Width = 400;
                dialog.Height = 300;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                // Показываем окно как модальное
                if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    await dialog.ShowDialog(desktop.MainWindow); // Используем MainWindow из IClassicDesktopStyleApplicationLifetime
                }
            }
        }


    }
}
