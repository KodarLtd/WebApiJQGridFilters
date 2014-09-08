using Kodar.JQGridFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIjqGridFiltersDemo.Models
{
    public class Company
    {
        [UrlParameter(UrlName="id")]
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}