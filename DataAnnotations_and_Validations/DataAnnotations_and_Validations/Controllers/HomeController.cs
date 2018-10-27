using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAnnotations_and_Validations.Models;

namespace DataAnnotations_and_Validations.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Employee emp)
        {
            // Modern way to validate in MVC 5
            if(ModelState.IsValid)
            {
                return RedirectToAction("Message");
            }

            //This is the traditiional way to validate
            //if(!string.IsNullOrEmpty(emp.username))
            //{
            //    return RedirectToAction("Message");
            //}
            //else
            //{
            //    ModelState.AddModelError("username","Please Enter Username");
            //    return View();
            //}      
            return View();
        }

        public ActionResult Message()
        {
            return View();
        }
    }
}