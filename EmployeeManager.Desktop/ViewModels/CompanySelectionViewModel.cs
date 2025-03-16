using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using EmployeeManager.Desktop.Services;
using EmployeeManager.Desktop.Views;
using EmployeeManager.Shared.DTOs.Company;
using ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using DynamicData;

namespace EmployeeManager.Desktop.ViewModels
{
    public class CompanySelectionViewModel : ReactiveObject
    {
        private readonly CompanyApiService _companyApiService;

        public ObservableCollection<CompanyReadDto> Companies { get; } = new();

        private CompanyReadDto? _selectedCompany;
        public CompanyReadDto? SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedCompany, value);
                this.RaisePropertyChanged(nameof(IsCompanySelected));
            }
        }

        public bool IsCompanySelected => SelectedCompany != null;

        public ICommand SelectCompanyCommand { get; }
        public ICommand RefreshCompaniesCommand { get; }
        public ICommand DeselectCompanyCommand { get; }
        public ICommand ViewCompanyInfoCommand { get; }

        public CompanySelectionViewModel() : this(App.Services.GetRequiredService<CompanyApiService>()) { }

        public CompanySelectionViewModel(CompanyApiService companyApiService)
        {
            _companyApiService = companyApiService;

            SelectCompanyCommand = ReactiveCommand.Create(SelectCompany, this.WhenAnyValue(x => x.IsCompanySelected));
            RefreshCompaniesCommand = ReactiveCommand.CreateFromTask(LoadCompaniesAsync);
            ViewCompanyInfoCommand = ReactiveCommand.CreateFromTask(ViewCompanyInfo, this.WhenAnyValue(x => x.IsCompanySelected));
            DeselectCompanyCommand = ReactiveCommand.Create(() => SelectedCompany = null);

            LoadCompaniesAsync().ConfigureAwait(false);
        }

        private async Task LoadCompaniesAsync()
        {
            var companies = await _companyApiService.GetCompaniesAsync().ConfigureAwait(false);
            Companies.Clear();
            Companies.AddRange(companies);
        }

        private void SelectCompany()
        {
            if (SelectedCompany == null) return;

            var employeeListView = new EmployeeListView
            {
                DataContext = new EmployeeListViewModel(SelectedCompany.Name)
            };

            if (App.Current != null && App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
                desktop.MainWindow is Window mainWindow)
            {
                mainWindow.Width = 880;
                mainWindow.Height = 630;
                MainWindowViewModel.Instance.CurrentView = employeeListView;
            }
        }

        private async Task ViewCompanyInfo()
        {
            if (SelectedCompany == null) return;

            var dialog = new CompanyInfoWindow(new CompanyInfoViewModel(SelectedCompany));

            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow != null)
            {
                await dialog.ShowDialog(desktop.MainWindow);
            }
        }
    }
}
