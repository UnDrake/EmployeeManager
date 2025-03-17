using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using EmployeeManager.Infrastructure;

namespace EmployeeManager.Data.Repositories
{
    public class DepartmentRepository : BaseRepository
    {
        public DepartmentRepository(DatabaseConnection databaseConnection, ILogger<DepartmentRepository> logger)
            : base(databaseConnection, logger) { }

        public async Task<int> GetOrCreateDepartmentAsync(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                throw new ArgumentException("Department name cannot be null or empty.", nameof(departmentName));

            string query = "SELECT ID FROM Departments WHERE Name = @DepartmentName";
            SqlParameter[] parameters =
            {
                new SqlParameter("@DepartmentName", departmentName),
            };

            int? existingId = await ExecuteScalarAsync<int?>(query, parameters);
            return existingId ?? await AddDepartmentAsync(departmentName);
        }

        private async Task<int> AddDepartmentAsync(string departmentName)
        {
            string query = "INSERT INTO Departments (Name) OUTPUT INSERTED.ID VALUES (@DepartmentName)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@DepartmentName", departmentName),
            };

            return await ExecuteScalarAsync<int>(query, parameters);
        }
    }
}