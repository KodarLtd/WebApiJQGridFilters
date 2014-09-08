using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebAPIjqGridFiltersDemo.Models;

namespace WebAPIjqGridFiltersDemo.Controllers
{
    public class BaseApiController : ApiController
    {
        #region Simulate Data Source

        private static List<Employee> employees;
        private static List<Company> companies;

        static BaseApiController()
        {
            companies = new List<Company>();
            employees = new List<Employee>();

            var microsoft = new Company { Name = "Microsoft", Address = "1 Microsoft Way, Seattle, USA", CompanyID = 1 };
            companies.Add(microsoft);
            var kodar = new Company { Name = "Kodar", Address = "9N Kuklensko Shose Str, Plovdiv, Bulgaria", CompanyID = 2 };
            companies.Add(kodar);
            var xamarin = new Company { Name = "Xamarin", Address = "San Francisco, USA", CompanyID = 3 };
            companies.Add(xamarin);
            var acme = new Company { Name = "Acme Corporation", Address = "USA", CompanyID = 4 };
            companies.Add(acme);

            var billGates = new Employee { Name = "Bill Gates", Company = microsoft, EmployeeID = 1, StartDate = new DateTime(1975, 1, 1), EndDate = new DateTime(2008, 6, 27) };
            employees.Add(billGates);
            var scottGu = new Employee { Name = "Scott Guthrie", Company = microsoft, EmployeeID = 2, StartDate = new DateTime(1997, 2, 2), EndDate = null };
            employees.Add(scottGu);
            var miguelDeIcaza = new Employee { Name = "Miguel DeIcaza", Company = xamarin, EmployeeID = 3, StartDate = new DateTime(2011, 5, 1), EndDate = null, };
            employees.Add(miguelDeIcaza);
            var asenRahnev = new Employee { Name = "Asen Rahnev", Company = kodar, EmployeeID = 4, StartDate = new DateTime(1996, 6, 6), EndDate = null };
            employees.Add(asenRahnev);
            var nikolayPavlov = new Employee { Name = "Nikolay Pavlov", Company = kodar, EmployeeID = 5, StartDate = new DateTime(1998, 6, 6), EndDate = null };
            employees.Add(nikolayPavlov);
            var testEmployee1 = new Employee { Name = "Test Employee 1", Company = acme, EmployeeID = 6, StartDate = new DateTime(2000, 7, 7), EndDate = new DateTime(2005, 6, 6) };
            employees.Add(testEmployee1);
            var testEmployee2 = new Employee { Name = "Test Employee 2", Company = acme, EmployeeID = 7, StartDate = new DateTime(2003, 7, 7), EndDate = new DateTime(2008, 6, 6) };
            employees.Add(testEmployee2);
            var testEmployee3 = new Employee { Name = "Test Employee 3", Company = acme, EmployeeID = 8, StartDate = new DateTime(2004, 7, 7), EndDate = new DateTime(2009, 6, 6) };
            employees.Add(testEmployee3);
            var testEmployee4 = new Employee { Name = "Test Employee 4", Company = acme, EmployeeID = 9, StartDate = new DateTime(2005, 7, 7), EndDate = new DateTime(2010, 6, 6) };
            employees.Add(testEmployee4);
            var testEmployee5 = new Employee { Name = "Test Employee 5", Company = acme, EmployeeID = 10, StartDate = new DateTime(2003, 9, 9), EndDate = new DateTime(2008, 6, 6) };
            employees.Add(testEmployee5);
            var testEmployee6 = new Employee { Name = "Test Employee 6", Company = acme, EmployeeID = 11, StartDate = new DateTime(2003, 7, 7), EndDate = new DateTime(2008, 6, 6) };
            employees.Add(testEmployee6);
        }

        public static IQueryable<Employee> Employees
        {
            get { return employees.AsQueryable(); }
        }

        public static IQueryable<Company> Companies
        {
            get { return companies.AsQueryable(); }
        }

        #endregion

    }
}