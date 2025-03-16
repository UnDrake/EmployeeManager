namespace EmployeeManager.Shared.DTOs.Company
{
    public class CompanyReadDto
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Info { get; set; }
    }
}
