using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Kodar.JQGridFilters.ActionParameters
{
    public class FilterTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            var filterString = value as string;
            if (filterString != null)
            {
                return Filter.Parse(filterString);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}