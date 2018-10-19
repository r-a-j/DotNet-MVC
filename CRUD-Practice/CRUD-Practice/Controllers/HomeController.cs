using CRUD_Practice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUD_Practice.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(EmployeeViewModel model)
        {
            using (var db=new MvcCRUDDBEntities())
            {
                Employee data = new Employee();
                data.Age = model.Age;
                data.Name = model.Name;
                data.Salary = model.Salary;
                data.Position = model.Position;
                data.Office = model.Office;
                db.Employee.Add(data);
                db.SaveChanges();
            }
                return View();
        }

        [HttpPost]
        public ActionResult AddData(EmployeeViewModel model)
        {
            using (var db = new MvcCRUDDBEntities())
            {
                Employee data = new Employee();
                data.Age = model.Age;
                data.Name = model.Name;
                data.Salary = model.Salary;
                data.Position = model.Position;
                data.Office = model.Office;
                db.Employee.Add(data);
                db.SaveChanges();
            }
            return View("Index");
        }
    }
}