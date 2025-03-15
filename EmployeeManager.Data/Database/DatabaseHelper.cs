using Microsoft.Data.SqlClient;

namespace EmployeeManager.Data.Database
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public void TestConnection()
        {
            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("✅ Подключение к базе данных успешно!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Ошибка подключения: {ex.Message}");
                }
            }
        }
    }
}
