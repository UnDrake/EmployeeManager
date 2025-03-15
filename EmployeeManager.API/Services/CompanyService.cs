using EmployeeManager.Data.Interfaces;
using EmployeeManager.Data.Repositories;
using EmployeeManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManager.API.Services
{
    public class CompanyService
    {
        private readonly CompanyRepository _companyRepository;

        public CompanyService(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        // ✅ Получение всех компаний
        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await Task.Run(() => _companyRepository.GetAll());
        }
    }
}
