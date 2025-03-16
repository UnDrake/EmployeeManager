using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using EmployeeManager.Infrastructure;


namespace EmployeeManager.Data.Repositories
{
    public class DepartmentRepository : BaseRepository
    {
        public DepartmentRepository(DatabaseConnection database, ILogger<DepartmentRepository> logger)
            : base(database, logger) { }

        public async Task<int> GetOrCreateDepartmentAsync(string departmentName, int companyID)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                throw new ArgumentException("Department name cannot be null or empty", nameof(departmentName));

            string query = "SELECT ID FROM Departments WHERE Name = @DepartmentName";
            SqlParameter[] parameters =
            {
                new SqlParameter("@DepartmentName", departmentName),
                new SqlParameter("@CompanyID", companyID)
            };

            int? existingId = await ExecuteScalarAsync<int?>(query, parameters);
            return existingId ?? await AddDepartmentAsync(departmentName, companyID);
        }

        private async Task<int> AddDepartmentAsync(string departmentName, int companyID)
        {
            string query = "INSERT INTO Departments (Name, CompanyID) OUTPUT INSERTED.ID VALUES (@DepartmentName, @CompanyID)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@DepartmentName", departmentName),
                new SqlParameter("@CompanyID", companyID)
            };

            return await ExecuteScalarAsync<int>(query, parameters);
        }
    }
}