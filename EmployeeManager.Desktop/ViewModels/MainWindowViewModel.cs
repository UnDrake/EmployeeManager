using EmployeeManager.Desktop.Views;
using ReactiveUI;

namespace EmployeeManager.Desktop.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private static MainWindowViewModel? _instance;
        public static MainWindowViewModel Instance => _instance ??= new MainWindowViewModel();

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        public MainWindowViewModel()
        {
            CurrentView = null;
        }

        public void Initialize()
        {
            if (CurrentView == null)
            {
                CurrentView = new CompanySelectionView();
            }
        }
    }

}
