using ReactiveUI;

namespace EmployeeManager.Desktop.ViewModels
{
    public class ReportSavedViewModel : ReactiveObject
    {
        public string FilePath { get; set; }

        public ReportSavedViewModel(string filePath)
        {
            FilePath = filePath;
        }
    }
}
