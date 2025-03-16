using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Shared.DTOs.Company
{
    public class CompanyCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Info { get; set; }
    }
}
