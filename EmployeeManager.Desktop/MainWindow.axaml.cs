using Avalonia.Controls;
using EmployeeManager.Desktop.ViewModels;

namespace EmployeeManager.Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel.Instance.Initialize();
            DataContext = MainWindowViewModel.Instance;
        }
    }
}
