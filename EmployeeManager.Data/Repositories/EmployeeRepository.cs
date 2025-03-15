using EmployeeManager.Data.Database;
using EmployeeManager.Data.Interfaces;
using Microsoft.Data.SqlClient;
using EmployeeManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManager.Data.Repositories
{
    public class EmployeeRepository : IBaseRepository<Employee>
    {
        private readonly DatabaseHelper _dbHelper;

        public EmployeeRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // ✅ Получение всех сотрудников
        public IEnumerable<Employee> GetAll()
        {
            var employees = new List<Employee>();

            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                string query = @"
                    SELECT e.ID, e.FullName, e.Phone, e.BirthDate, e.HireDate, e.Salary,
                           p.Name AS Position, d.Name AS Department, 
                           a.Address
                    FROM Employees e
                    JOIN Positions p ON e.PositionID = p.ID
                    JOIN Departments d ON p.DepartmentID = d.ID
                    JOIN Addresses a ON e.AddressID = a.ID";

                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            ID = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Phone = reader.IsDBNull(2) ? null : reader.GetString(2),
                            BirthDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                            HireDate = reader.GetDateTime(4),
                            Salary = reader.GetDecimal(5),
                            Position = reader.GetString(6),
                            Department = reader.GetString(7),
                            Address = reader.GetString(8)
                        });
                    }
                }
            }
            return employees;
        }

        // ✅ Добавление сотрудника
        public async Task Create(Employee entity)
        {
            int departmentID = GetOrCreateDepartment(entity.Department);
            int positionID = GetOrCreatePosition(entity.Position, departmentID);
            int addressID = GetOrCreateAddress(entity.Address);

            using (var conn = _dbHelper.GetConnection())
            {
                await conn.OpenAsync();
                string query = @"INSERT INTO Employees 
                                (FullName, Phone, BirthDate, HireDate, Salary, PositionID, AddressID) 
                                VALUES (@FullName, @Phone, @BirthDate, @HireDate, @Salary, @PositionID, @AddressID)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", entity.FullName);
                    cmd.Parameters.AddWithValue("@Phone", (object)entity.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@BirthDate", (object)entity.BirthDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@HireDate", entity.HireDate);
                    cmd.Parameters.AddWithValue("@Salary", entity.Salary);
                    cmd.Parameters.AddWithValue("@PositionID", positionID);
                    cmd.Parameters.AddWithValue("@AddressID", addressID);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // ✅ Обновление сотрудника
        public async Task<Employee> Update(Employee entity)
        {
            int departmentID = GetOrCreateDepartment(entity.Department);
            int positionID = GetOrCreatePosition(entity.Position, departmentID);
            int addressID = GetOrCreateAddress(entity.Address);

            using (var conn = _dbHelper.GetConnection())
            {
                await conn.OpenAsync();
                string query = @"UPDATE Employees 
                                 SET FullName = @FullName, Phone = @Phone, BirthDate = @BirthDate, 
                                     HireDate = @HireDate, Salary = @Salary, 
                                     PositionID = @PositionID, AddressID = @AddressID
                                 WHERE ID = @ID";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", entity.ID);
                    cmd.Parameters.AddWithValue("@FullName", entity.FullName);
                    cmd.Parameters.AddWithValue("@Phone", (object)entity.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@BirthDate", (object)entity.BirthDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@HireDate", entity.HireDate);
                    cmd.Parameters.AddWithValue("@Salary", entity.Salary);
                    cmd.Parameters.AddWithValue("@PositionID", positionID);
                    cmd.Parameters.AddWithValue("@AddressID", addressID);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return entity;
        }

        // ✅ Удаление сотрудника
        public async Task Delete(Employee entity)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                await conn.OpenAsync();
                string query = "DELETE FROM Employees WHERE ID = @ID";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", entity.ID);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // 🔹 Вспомогательные методы

        private int GetOrCreateDepartment(string departmentName)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT ID FROM Departments WHERE Name = @DepartmentName";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentName", departmentName);
                    object result = cmd.ExecuteScalar();
                    return result != null ? (int)result : AddDepartment(departmentName);
                }
            }
        }

        private int AddDepartment(string departmentName)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Departments (Name) OUTPUT INSERTED.ID VALUES (@DepartmentName)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentName", departmentName);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        private int GetOrCreatePosition(string positionName, int departmentID)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT ID FROM Positions WHERE Name = @PositionName AND DepartmentID = @DepartmentID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PositionName", positionName);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                    object result = cmd.ExecuteScalar();
                    return result != null ? (int)result : AddPosition(positionName, departmentID);
                }
            }
        }

        private int AddPosition(string positionName, int departmentID)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Positions (Name, DepartmentID) OUTPUT INSERTED.ID VALUES (@PositionName, @DepartmentID)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PositionName", positionName);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        private int GetOrCreateAddress(string address)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT ID FROM Addresses WHERE Address = @Address";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Address", address);
                    object result = cmd.ExecuteScalar();
                    return result != null ? (int)result : AddAddress(address);
                }
            }
        }

        private int AddAddress(string address)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Addresses (Address) OUTPUT INSERTED.ID VALUES (@Address)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Address", address);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
