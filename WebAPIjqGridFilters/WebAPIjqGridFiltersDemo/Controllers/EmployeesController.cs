using Kodar.JQGridFilters;
using Kodar.JQGridFilters.ActionParameters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIjqGridFiltersDemo.Models;

namespace WebAPIjqGridFiltersDemo.Controllers
{
    public class EmployeesController : BaseApiController
    {
        // GET /Employees/
        public ApiResult<EmployeeDto> Get(int rows, int page, string sidx, string sord)
        {
            return GetDtoResult(Employees, rows, page, sidx, sord, null, c => new EmployeeDto(c));
        }

        // GET /Employees/
        public ApiResult<EmployeeDto> Get(int rows, int page, string sidx, string sord, [FromUri]Filter filters)
        {
            return GetDtoResult(Employees, rows, page, sidx, sord, filters, c => new EmployeeDto(c));
        }
    }
}
