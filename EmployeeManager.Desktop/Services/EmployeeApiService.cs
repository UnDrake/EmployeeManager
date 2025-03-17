using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EmployeeManager.Shared.DTOs.Employee;
using EmployeeManager.Desktop.Utils;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Desktop.Services
{
    public class EmployeeApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmployeeApiService> _logger;

        public EmployeeApiService(HttpClient httpClient, ILogger<EmployeeApiService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<EmployeeReadDto>> GetEmployeesByCompanyAsync(string companyName)
        {
            try
            {
                var url = string.Format(ApiEndpoints.EmployeesByCompany, companyName);
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error getting employees for company {Company}. Code: {StatusCode}", companyName, response.StatusCode);
                    return new List<EmployeeReadDto>();
                }

                return await response.Content.ReadFromJsonAsync<List<EmployeeReadDto>>() ?? new List<EmployeeReadDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while querying employees by company");
                return new List<EmployeeReadDto>();
            }
        }

        public async Task<int> AddEmployeeAsync(EmployeeCreateDto employee)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.Employees, employee);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error adding employee. Code: {StatusCode}", response.StatusCode);
                    return 0;
                }

                var createdEmployee = await response.Content.ReadFromJsonAsync<EmployeeReadDto>();
                return createdEmployee?.ID ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding employee");
                return 0;
            }
        }

        public async Task<bool> UpdateEmployeeAsync(EmployeeUpdateDto employee)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{ApiEndpoints.Employees}", employee);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee");
                return false;
            }
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{ApiEndpoints.Employees}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee.");
                return false;
            }
        }
    }
}
