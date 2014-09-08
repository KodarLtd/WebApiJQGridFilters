using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kodar.JQGridFilters
{
    public class FilterExpressionException : Exception
    {
        public FilterExpressionException(string message)
            : base(message)
        { }

        public FilterExpressionException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}