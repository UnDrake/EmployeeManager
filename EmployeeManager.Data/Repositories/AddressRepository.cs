using EmployeeManager.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;


namespace EmployeeManager.Data.Repositories
{
    public class AddressRepository : BaseRepository
    {
        public AddressRepository(DatabaseConnection database, ILogger<AddressRepository> logger)
            : base(database, logger) { }

        public async Task<int> GetOrCreateAddressAsync(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address cannot be null or empty", nameof(address));

            string query = "SELECT ID FROM Addresses WHERE Address = @Address";
            SqlParameter[] parameters = { new SqlParameter("@Address", address) };

            int? existingId = await ExecuteScalarAsync<int?>(query, parameters);
            return existingId ?? await AddAddressAsync(address);
        }

        private async Task<int> AddAddressAsync(string address, CancellationToken cancellationToken = default)
        {
            string query = "INSERT INTO Addresses (Address) OUTPUT INSERTED.ID VALUES (@Address)";
            SqlParameter[] parameters = { new SqlParameter("@Address", address) };

            return await ExecuteScalarAsync<int>(query, parameters);
        }
    }
}