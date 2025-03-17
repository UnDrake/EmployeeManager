using System.Reflection;

namespace EmployeeManager.Shared.ModelsToSave
{
    public class EmployeeListElement : EmployeeBaseElement
    {
        public string Phone { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; } = string.Empty;

        public override IEnumerable<object> GetFields()
        {
            return base.GetFields().Concat([Phone, BirthDate, Address]);
        }

        public override string[] GetFieldNames()
        {
            return GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(prop => prop.Name)
                .ToArray();
        }
    }
}
