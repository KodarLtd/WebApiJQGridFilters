using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace Kodar.JQGridFilters.ActionParameters
{

    [TypeConverter(typeof(FilterTypeConverter))]
    [DataContract]
    public class Filter
    {
        [DataMember(Name = "groupOp")]
        public string GroupOp { get; set; }
        [DataMember(Name = "rules")]
        public FilterRule[] Rules { get; set; }

        public static Filter Parse(string s)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Filter));
                    return (Filter)serializer.ReadObject(stream);
                }
            }
            catch (SerializationException)
            {
                return null;
            }
        }
    }
}