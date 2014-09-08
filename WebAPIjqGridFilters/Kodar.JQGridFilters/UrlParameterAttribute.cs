using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kodar.JQGridFilters
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UrlParameterAttribute : Attribute
    {
        public string UrlName { get; set; }

        private bool allowed = true;
        public bool Allowed
        {
            get { return allowed; }
            set { allowed = value; }
        }
    }
}