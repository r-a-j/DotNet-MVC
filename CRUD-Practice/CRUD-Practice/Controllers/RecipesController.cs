using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUD_Practice.Controllers
{
    public class RecipesController : Controller
    {
        // GET: Recipes
        public ActionResult Index()
        {
            return View();
        }
    }
}