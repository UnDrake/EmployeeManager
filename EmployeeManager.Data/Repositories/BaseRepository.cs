using EmployeeManager.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Data.Repositories
{
    public abstract class BaseRepository
    {
        private readonly DatabaseConnection _databaseConnection;
        private readonly ILogger<BaseRepository> _logger;

        protected BaseRepository(DatabaseConnection database, ILogger<BaseRepository> logger)
        {
            _databaseConnection = database ?? throw new ArgumentNullException(nameof(database));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected async Task<T?> ExecuteScalarAsync<T>(string query, SqlParameter[] parameters)
        {
            try
            {
                using (var conn = _databaseConnection.GetConnection())
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        object? result = await cmd.ExecuteScalarAsync();
                        return result is T value ? value : default;
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error occurred while executing scalar query: {Query}.", query);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation while executing scalar query: {Query}.", query);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while executing scalar query: {Query}.", query);
                throw;
            }
        }

        protected async Task<int> ExecuteNonQueryAsync(string query, SqlParameter[] parameters)
        {
            try
            {
                using (var conn = _databaseConnection.GetConnection())
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        return await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error occurred while executing non-query: {Query}.", query);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation while executing non-query: {Query}.", query);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while executing non-query: {Query}.", query);
                throw;
            }
        }

        protected async Task<List<T>> ExecuteReaderAsync<T>(string query, SqlParameter[] parameters, Func<SqlDataReader, T> map)
        {
            var results = new List<T>();
            try
            {
                using (var conn = _databaseConnection.GetConnection())
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                results.Add(map(reader));
                            }
                        }
                    }
                }
                return results;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error occurred while executing reader query: {Query}.", query);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation while executing reader query: {Query}.", query);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while executing reader query: {Query}.", query);
                throw;
            }
        }
    }
}