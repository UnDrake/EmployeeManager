using EmployeeManager.Core.Interfaces;
using EmployeeManager.Shared.DTOs.Company;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<CompaniesController> _logger;

        public CompaniesController(ICompanyService companyService, ILogger<CompaniesController> logger)
        {
            _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyReadDto>>> GetCompanies()
        {
            _logger.LogInformation("Fetching all companies.");

            var companies = await _companyService.GetAllCompaniesAsync();

            if (companies == null || !companies.Any())
            {
                _logger.LogWarning("No companies found.");
                return NotFound("No companies available.");
            }

            return Ok(companies);
        }
    }
}
