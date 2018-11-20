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
            using (var db = new MvcCRUDDBEntities())
            {
                Employee data = new Employee();
                data.EmployeeName = model.EmployeeName;
                data.EmployeePosition = model.EmployeePosition;
                data.EmployeeNumber = model.EmployeeNumber;
                data.EmployeeSalary = model.EmployeeSalary;
                data.EmployeeAddress = model.EmployeeAddress;
                data.EmployeeEmail = model.EmployeeEmail;
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
                data.EmployeeName = model.EmployeeName;
                data.EmployeePosition = model.EmployeePosition;
                data.EmployeeNumber = model.EmployeeNumber;
                data.EmployeeSalary = model.EmployeeSalary;
                data.EmployeeAddress = model.EmployeeAddress;
                data.EmployeeEmail = model.EmployeeEmail;
                db.Employee.Add(data);
                db.SaveChanges();
            }
            return View("Index");
        }
    }
}