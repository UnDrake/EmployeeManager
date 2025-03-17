using Avalonia.Controls.ApplicationLifetimes;
using ConsoleTables;
using EmployeeManager.Desktop.ViewModels;
using EmployeeManager.Desktop.Views;
using EmployeeManager.Shared.ModelsToSave;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Desktop.Utils
{
    public class ReportSaver
    {
        private readonly ILogger<ReportSaver> _logger;

        public ReportSaver(ILogger<ReportSaver> logger)
        {
            _logger = logger;
        }

        public async Task SaveReportAsync(List<EmployeeBaseElement> employees, string filePath = "report.txt")
        {
            if (employees == null || employees.Count == 0)
            {
                _logger.LogWarning("No employees available to generate the report.");
                await ShowReportSavedWindow("No employees available.");
                return;
            }

            try
            {
                string[] headers = employees.First().GetFieldNames();
                var table = new ConsoleTable(headers);

                foreach (var employee in employees)
                {
                    table.AddRow(employee.GetFields().ToArray());
                }

                string tableContent = table.ToString();
                File.WriteAllText(filePath, tableContent);

                _logger.LogInformation($"Report saved successfully to {filePath}.");

                await ShowReportSavedWindow($"{Path.GetFullPath(filePath)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving report to {FilePath}.", filePath);
                await ShowReportSavedWindow($"{ex.Message}");
            }
        }

        private async Task ShowReportSavedWindow(string message)
        {
            var dialog = new ReportSavedWindow(new ReportSavedViewModel(message));

            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow != null)
            {
                await dialog.ShowDialog(desktop.MainWindow);
            }
        }
    }
}
