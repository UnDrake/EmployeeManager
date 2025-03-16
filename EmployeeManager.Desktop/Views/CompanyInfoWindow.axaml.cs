using Avalonia.Controls;
using EmployeeManager.Desktop.ViewModels;

namespace EmployeeManager.Desktop.Views
{
    public partial class CompanyInfoWindow : Window
    {
        public CompanyInfoWindow(CompanyInfoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
