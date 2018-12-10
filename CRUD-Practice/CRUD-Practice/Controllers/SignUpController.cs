using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUD_Practice.Models;

namespace CRUD_Practice.Controllers
{
    public class SignUpController : Controller
    {
        // GET: SignUp

        public ActionResult Index()
        {			
			return View();
        }


			[HttpPost]
			public ActionResult Index(CustomerViewModel model)
			{
				using (var db = new MvcCRUDDBEntities1())
				{
					Customer data = new Customer();
					data.CustomerName = model.CustomerName;
					data.CustomerEmail = model.CustomerEmail;
					data.CustomerPhone = model.CustomerPhone;
					data.CustomerPassword = model.CustomerPassword;
					var passwordCheck = model.CustomerRePassword;
					data.CustomerAddress = model.CustomerAddress;					
					db.Customers.Add(data);
					db.SaveChanges();	
				
				}

			return RedirectToAction("Index", "Profile");
			//return View("Index","Profile");
		}
	}
}