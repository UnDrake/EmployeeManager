using System.Reflection;

namespace EmployeeManager.Shared.ModelsToSave
{
    public abstract class EmployeeBaseElement
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime? HireDate { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;

        public virtual IEnumerable<object> GetFields()
        {
            return [FullName, HireDate, Position, Department, Company ];
        }

        public abstract string[] GetFieldNames();
    }
}
