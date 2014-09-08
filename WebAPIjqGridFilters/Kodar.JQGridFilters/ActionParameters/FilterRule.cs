using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kodar.JQGridFilters.ActionParameters
{
    [DataContract]
    public class FilterRule
    {
        [DataMember(Name = "field")]
        public string Field { get; set; }
        [DataMember(Name = "op")]
        public string Operation { get; set; }
        [DataMember(Name = "data")]
        public string Data { get; set; }
    }
}