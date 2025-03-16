using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using EmployeeManager.Infrastructure;


namespace EmployeeManager.Data.Repositories
{
    public class PositionRepository : BaseRepository
    {
        public PositionRepository(DatabaseConnection database, ILogger<PositionRepository> logger)
            : base(database, logger) { }

        public async Task<int> GetOrCreatePositionAsync(string positionName, int departmentID)
        {
            if (string.IsNullOrWhiteSpace(positionName))
                throw new ArgumentException("Position name cannot be null or empty", nameof(positionName));

            string query = "SELECT ID FROM Positions WHERE Name = @PositionName AND DepartmentID = @DepartmentID";
            SqlParameter[] parameters =
            {
                new SqlParameter("@PositionName", positionName),
                new SqlParameter("@DepartmentID", departmentID)
            };

            int? existingId = await ExecuteScalarAsync<int?>(query, parameters);
            return existingId ?? await AddPositionAsync(positionName, departmentID);
        }

        private async Task<int> AddPositionAsync(string positionName, int departmentID)
        {
            string query = "INSERT INTO Positions (Name, DepartmentID) OUTPUT INSERTED.ID VALUES (@PositionName, @DepartmentID)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@PositionName", positionName),
                new SqlParameter("@DepartmentID", departmentID)
            };

            return await ExecuteScalarAsync<int>(query, parameters);
        }
    }
}