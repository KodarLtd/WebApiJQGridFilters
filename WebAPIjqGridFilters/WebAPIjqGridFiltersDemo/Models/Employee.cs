using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIjqGridFiltersDemo.Models
{
    public class Employee
    {
        //[UrlParameter(UrlName="ID")]
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Company Company { get; set; }
    }
}