namespace EmployeeManager.Shared.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public decimal Salary { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
    }
}
