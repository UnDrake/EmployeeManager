namespace EmployeeManager.Shared.DTOs.Employee
{
    public class EmployeeBaseDto
    {
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
    }
}
