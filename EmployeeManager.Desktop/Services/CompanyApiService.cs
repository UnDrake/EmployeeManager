using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EmployeeManager.Shared.DTOs.Company;
using EmployeeManager.Desktop.Utils;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Desktop.Services
{
    public class CompanyApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CompanyApiService> _logger;

        public CompanyApiService(HttpClient httpClient, ILogger<CompanyApiService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<CompanyReadDto>> GetCompaniesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(ApiEndpoints.Companies);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error getting list of companies. Code: {StatusCode}.", response.StatusCode);
                    return new List<CompanyReadDto>();
                }

                return await response.Content.ReadFromJsonAsync<List<CompanyReadDto>>() ?? new List<CompanyReadDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while querying companies.");
                return new List<CompanyReadDto>();
            }
        }
    }
}
