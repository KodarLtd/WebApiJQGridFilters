using Kodar.JQGridFilters;
using Kodar.JQGridFilters.ActionParameters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPIjqGridFiltersDemo.Models;

namespace WebAPIjqGridFiltersDemo.Controllers
{
    public class CompaniesController : BaseApiController
    {
        // GET /Companies/
        public JObject Get(int rows, int page, string sidx, string sord)
        {
            return GetCompanies(rows, page, sidx, sord, null);
        }

        // GET /Companies/
        public JObject Get(int rows, int page, string sidx, string sord, [FromUri]Filter filters)
        {
            return GetCompanies(rows, page, sidx, sord, filters);
        }

        private JObject GetCompanies(int rows, int page, string sidx, string sord, Filter filters)
        {
            IQueryable<Company> items = Companies.SortBy(filters, sidx, sord);

            int totalItems = items.Count();
            //in a real app Count should probably be retrieved asynchronously
            //int totalItems = await items.CountAsync();
            int totalPages = totalItems / rows;
            if (totalItems % rows > 0)
            {
                totalPages++;
            }

            items = items.Skip((page - 1) * rows).Take(rows);

            List<Company> listOfItems = items.ToList();
            //List<Company> listOfItems = await items.ToListAsync();

            dynamic result = new JObject();
            result.total = totalPages;
            result.records = totalItems;
            result.page = page;
            result.rows = new JArray(listOfItems.Select(c =>
                {
                    dynamic o = new JObject();
                    o.id = c.CompanyID;
                    o.cell = new JArray(c.CompanyID, c.Name, c.Address);
                    return o;
                }).ToArray());

            return result;
        }
    }
}
