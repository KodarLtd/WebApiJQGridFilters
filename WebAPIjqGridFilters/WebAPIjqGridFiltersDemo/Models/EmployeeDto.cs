using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIjqGridFiltersDemo.Models
{
    public class EmployeeDto
    {
        public EmployeeDto(Employee employee)
        {
            Name = employee.Name;
            StartDate = employee.StartDate;
            EndDate = employee.EndDate;
            CompanyName = employee.Company.Name;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CompanyName { get; set; }
    }
}