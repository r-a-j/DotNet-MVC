using Entity_Framework_Conventions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Entity_Framework_Conventions.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            EmployeeContext db = new EmployeeContext();
            var data = db.Employees.ToList();
            return View();
        }
    }
}