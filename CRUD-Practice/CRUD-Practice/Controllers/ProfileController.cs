using CRUD_Practice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUD_Practice.Controllers
{
	public class ProfileController : Controller
	{
		// GET: Profile
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Index(CustomerViewModel model)
		{
			using (var db = new MvcCRUDDBEntities1())
			{
				var userData = db.tblCustomer.Where(a => a.CustomerEmail == model.CustomerEmail && a.CustomerPassword == model.CustomerPassword).FirstOrDefault();
				if (userData != null)
				{
					return RedirectToAction("Index", "Home");
				}
				else
				{
					return RedirectToAction("Index", "Profile");
				}
			}		
		}
	}
}