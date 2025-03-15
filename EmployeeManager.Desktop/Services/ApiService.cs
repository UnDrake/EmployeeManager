using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using EmployeeManager.Models;

namespace EmployeeManager.Desktop.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7240/api/") };
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            var em = await _httpClient.GetFromJsonAsync<List<Employee>>("employees") ?? new List<Employee>();
            return em;
        }

        public async Task<int> AddEmployeeAsync(Employee employee)
        {
            var response = await _httpClient.PostAsJsonAsync("employees", employee);

            if (response.IsSuccessStatusCode)
            {
                var createdEmployee = await response.Content.ReadFromJsonAsync<Employee>();
                return createdEmployee?.ID ?? 0; 
            }

            return 0; // ❌ Ошибка добавления
        }


        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            var response = await _httpClient.PutAsJsonAsync($"employees", employee);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"employees/{id}");
            return response.IsSuccessStatusCode;
        }

    }
}
