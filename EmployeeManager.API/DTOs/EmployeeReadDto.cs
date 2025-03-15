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

    }
}
