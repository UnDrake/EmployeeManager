using EmployeeManager.Models;

namespace EmployeeManager.API.DTOs
{
    public class EmployeeReadDto
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }

        public string Position { get; set; }  // Название должности
        public string Department { get; set; }  // Название отдела
        public string Address { get; set; }  // Полный адрес (Город + Улица)
        public string Company { get; set; } // ✅ Компания

        public EmployeeReadDto(Employee employee)
        {
            ID = employee.ID;
            FullName = employee.FullName;
            Phone = employee.Phone;
            BirthDate = employee.BirthDate;
            HireDate = employee.HireDate;
            Salary = employee.Salary;
            Position = employee.Position;
            Department = employee.Department;
            Company = employee.Company;
            Address = employee.Address;
        }

        public EmployeeReadDto() { }
    }
}
