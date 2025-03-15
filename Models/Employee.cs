namespace EmployeeManager.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }

        public string Position { get; set; }  // ✅ Должность
        public string Department { get; set; }  // ✅ Отдел
        public string Address { get; set; }  // ✅ Адрес
        public string Company { get; set; }
    }

}
