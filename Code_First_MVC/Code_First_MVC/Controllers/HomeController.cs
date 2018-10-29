using Code_First_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Code_First_MVC.Controllers
{    
    public class HomeController : Controller
    {
        EmployeeContext db = new EmployeeContext();
        public ActionResult Index()
        {
            var data1 = db.Employees.ToList();
            return View(data1);
        }        
    }
}