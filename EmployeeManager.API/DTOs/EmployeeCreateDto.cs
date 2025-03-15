namespace EmployeeManager.API.DTOs
{
    public class EmployeeCreateDto
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }

        public string Position { get; set; }  // Название должности (вместо ID)
        public string Department { get; set; }  // Название отдела
        public string Address { get; set; }  // Город
        public string Company { get; set; } // ✅ Компания


    }
}
