using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PartialViews_in_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }               

        public PartialViewResult Category()
        {
            string[] books = { "PHP", "Java", "C++", "Android" };
            return PartialView("_CateogryDynamic", books);
        }
    }
}