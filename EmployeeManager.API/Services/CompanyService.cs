using EmployeeManager.Data.Repositories;
using EmployeeManager.Models;

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
            return await Task.Run(() => _companyRepository.GetAllAsync());
        }
    }
}
