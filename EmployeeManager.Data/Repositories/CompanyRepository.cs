using EmployeeManager.Data.Database;
using EmployeeManager.Data.Interfaces;
using EmployeeManager.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManager.Data.Repositories
{
    public class CompanyRepository 
    {
        private readonly DatabaseHelper _dbHelper;

        public CompanyRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // ✅ Получение всех компаний
        public IEnumerable<Company> GetAll()
        {
            var companies = new List<Company>();

            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT ID, Name FROM Companies";

                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        companies.Add(new Company
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
            return companies;
        }
    }
}
