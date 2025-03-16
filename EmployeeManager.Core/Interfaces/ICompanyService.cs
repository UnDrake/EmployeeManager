using EmployeeManager.Shared.DTOs.Company;

namespace EmployeeManager.Core.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyReadDto>> GetAllCompaniesAsync();
    }
}
