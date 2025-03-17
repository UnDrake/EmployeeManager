using System.Reflection;

namespace EmployeeManager.Shared.ModelsToSave
{
    public class EmployeePayrollElement : EmployeeBaseElement
    {
        public decimal Salary { get; set; }

        public override IEnumerable<object> GetFields()
        {
            return base.GetFields().Concat([Salary]);
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
