using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TempData_and_Session.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TempData["mydata"] = DateTime.Now.ToString();
            return RedirectToAction("Index1");
        }

        public ActionResult Index1()
        {
            TempData.Keep();
            return View();
        }

        public ActionResult Index2()
        {
            Session["mysession"] = "DotNet Practicals";
            return View();
        }


    }
}