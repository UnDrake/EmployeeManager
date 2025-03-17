using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EmployeeManager.Desktop.ViewModels;

namespace EmployeeManager.Desktop.Views;

public partial class ReportSavedWindow : Window
{
    public ReportSavedWindow(ReportSavedViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}