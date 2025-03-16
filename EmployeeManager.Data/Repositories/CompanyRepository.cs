using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using EmployeeManager.Models;
using EmployeeManager.Infrastructure;


namespace EmployeeManager.Data.Repositories
{
    public class CompanyRepository : BaseRepository
    {
        public CompanyRepository(DatabaseConnection database, ILogger<CompanyRepository> logger) 
            : base(database, logger) { }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            string query = "SELECT ID, Name, Info FROM Companies";
            return await ExecuteReaderAsync(query, new SqlParameter[] { }, reader => new Company
            {
                ID = reader.GetInt32(0),
                Name = reader.GetString(1),
                Info = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
            });
        }

        public async Task<int> GetOrCreateCompanyAsync(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName))
                throw new ArgumentException("Company name cannot be null or empty", nameof(companyName));

            string query = "SELECT ID FROM Companies WHERE Name = @CompanyName";
            SqlParameter[] parameters = { new SqlParameter("@CompanyName", companyName) };

            int? existingId = await ExecuteScalarAsync<int?>(query, parameters);
            return existingId ?? await AddCompanyAsync(companyName);
        }

        private async Task<int> AddCompanyAsync(string companyName)
        {
            string query = "INSERT INTO Companies (Name) OUTPUT INSERTED.ID VALUES (@CompanyName)";
            SqlParameter[] parameters = { new SqlParameter("@CompanyName", companyName) };

            return await ExecuteScalarAsync<int>(query, parameters);
        }
    }
}