using EmployeeManager.Core.Interfaces;
using EmployeeManager.Data.Repositories;
using EmployeeManager.Shared.DTOs.Company;

namespace EmployeeManager.Core.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly CompanyRepository _companyRepository;

        public CompanyService(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<CompanyReadDto>> GetAllCompaniesAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            return companies.Select(c => new CompanyReadDto
            {
                ID = c.ID,
                Name = c.Name,
                Info = c.Info
            });
        }
    }

}
