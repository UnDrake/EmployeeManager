using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using EmployeeManager.Infrastructure;
using EmployeeManager.Shared.Models;

namespace EmployeeManager.Data.Repositories
{
    public class EmployeeRepository : BaseRepository
    {
        private readonly CompanyRepository _companyRepository;
        private readonly DepartmentRepository _departmentRepository;
        private readonly PositionRepository _positionRepository;
        private readonly AddressRepository _addressRepository;

        public EmployeeRepository(DatabaseConnection database,
                                  CompanyRepository companyRepository,
                                  DepartmentRepository departmentRepository,
                                  PositionRepository positionRepository,
                                  AddressRepository addressRepository,
                                  ILogger<EmployeeRepository> logger) : base(database, logger)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _positionRepository = positionRepository ?? throw new ArgumentNullException(nameof(positionRepository));
            _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            string query = @"
                SELECT e.ID, e.FullName, e.Phone, e.BirthDate, e.HireDate, e.Salary,
                       p.Name AS Position, d.Name AS Department, c.Name AS Company,
                       a.Address
                FROM Employees e
                JOIN Positions p ON e.PositionID = p.ID
                JOIN Departments d ON p.DepartmentID = d.ID
                JOIN Companies c ON e.CompanyID = c.ID
                JOIN Addresses a ON e.AddressID = a.ID";

            return await ExecuteReaderAsync(query, Array.Empty<SqlParameter>(), reader => new Employee
            {
                ID = reader.GetInt32(0),
                FullName = reader.GetString(1),
                Phone = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                BirthDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                HireDate = reader.GetDateTime(4),
                Salary = reader.GetDecimal(5),
                Position = reader.GetString(6),
                Department = reader.GetString(7),
                Company = reader.GetString(8),
                Address = reader.GetString(9)
            });
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByCompanyAsync(string companyName)
        {
            string query = @"
                SELECT e.ID, e.FullName, e.Phone, e.BirthDate, e.HireDate, e.Salary,
                       p.Name AS Position, d.Name AS Department, c.Name AS Company,
                       a.Address
                FROM Employees e
                JOIN Positions p ON e.PositionID = p.ID
                JOIN Departments d ON p.DepartmentID = d.ID
                JOIN Companies c ON e.CompanyID = c.ID
                JOIN Addresses a ON e.AddressID = a.ID
                WHERE c.Name = @CompanyName";

            SqlParameter[] parameters = { new SqlParameter("@CompanyName", companyName) };

            return await ExecuteReaderAsync(query, parameters, reader => new Employee
            {
                ID = reader.GetInt32(0),
                FullName = reader.GetString(1),
                Phone = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                BirthDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                HireDate = reader.GetDateTime(4),
                Salary = reader.GetDecimal(5),
                Position = reader.GetString(6),
                Department = reader.GetString(7),
                Company = reader.GetString(8),
                Address = reader.GetString(9)
            });
        }

        public async Task<int> CreateAsync(Employee entity)
        {
            int companyID = await _companyRepository.GetOrCreateCompanyAsync(entity.Company);
            int departmentID = await _departmentRepository.GetOrCreateDepartmentAsync(entity.Department, companyID);
            int positionID = await _positionRepository.GetOrCreatePositionAsync(entity.Position, departmentID);
            int addressID = await _addressRepository.GetOrCreateAddressAsync(entity.Address);

            string query = @"
                INSERT INTO Employees 
                (FullName, Phone, BirthDate, HireDate, Salary, PositionID, AddressID, CompanyID) 
                OUTPUT INSERTED.ID
                VALUES (@FullName, @Phone, @BirthDate, @HireDate, @Salary, @PositionID, @AddressID, @CompanyID)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@FullName", entity.FullName),
                new SqlParameter("@Phone", (object ?)entity.Phone ?? DBNull.Value),
                new SqlParameter("@BirthDate", (object ?)entity.BirthDate ?? DBNull.Value),
                new SqlParameter("@HireDate", entity.HireDate),
                new SqlParameter("@Salary", entity.Salary),
                new SqlParameter("@PositionID", positionID),
                new SqlParameter("@AddressID", addressID),
                new SqlParameter("@CompanyID", companyID)
            };

            return await ExecuteScalarAsync<int>(query, parameters);
        }

        public async Task<bool> UpdateAsync(Employee entity)
        {
            int companyID = await _companyRepository.GetOrCreateCompanyAsync(entity.Company);
            int departmentID = await _departmentRepository.GetOrCreateDepartmentAsync(entity.Department, companyID);
            int positionID = await _positionRepository.GetOrCreatePositionAsync(entity.Position, departmentID);
            int addressID = await _addressRepository.GetOrCreateAddressAsync(entity.Address);

            string query = @"
                UPDATE Employees 
                SET FullName = @FullName, Phone = @Phone, BirthDate = @BirthDate, 
                    HireDate = @HireDate, Salary = @Salary, 
                    PositionID = @PositionID, AddressID = @AddressID, CompanyID = @CompanyID
                WHERE ID = @ID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ID", entity.ID),
                new SqlParameter("@FullName", entity.FullName),
                new SqlParameter("@Phone", (object ?)entity.Phone ?? DBNull.Value),
                new SqlParameter("@BirthDate", (object ?)entity.BirthDate ?? DBNull.Value),
                new SqlParameter("@HireDate", entity.HireDate),
                new SqlParameter("@Salary", entity.Salary),
                new SqlParameter("@PositionID", positionID),
                new SqlParameter("@AddressID", addressID),
                new SqlParameter("@CompanyID", companyID)
            };

            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<bool> DeleteAsync(int employeeId)
        {
            string query = "DELETE FROM Employees WHERE ID = @ID";
            SqlParameter[] parameters = { new SqlParameter("@ID", employeeId) };

            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }
    }
}
