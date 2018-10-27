using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ViewDataViewBag.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Action1()
        {
            ViewData["viewdata1"] = DateTime.Now.ToString();
            ViewBag.viewbag1 = DateTime.Now.ToString();
            return View();
        }

        
    }
}