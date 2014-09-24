using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIjqGridFiltersDemo.Models
{
    public class ApiResult<TResult>
    {
        //arbitrary fields here in addition to the actual records
        public int totalPages { get; set; }
        public int totalRows { get; set; }
        public int rowsPerPage { get; set; }
        public string sortCol { get; set; }
        public string sortDir { get; set; }
        public int startRow { get; set; }
        public IEnumerable<TResult> records { get; set; }
    }
}