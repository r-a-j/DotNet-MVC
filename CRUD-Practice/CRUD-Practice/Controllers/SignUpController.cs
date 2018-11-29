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
				using (var db = new MvcCRUDDBEntities())
				{
					Customer data = new Customer();
					data.CustomerName = model.CustomerName;
					data.CustomerEmail = model.CustomerEmail;
					data.CustomerPhone = model.CustomerPhone;
					data.CustomerPassword = model.CustomerPassword;
					data.CustomerAddress = model.CustomerAddress;					
					db.Customer.Add(data);
					db.SaveChanges();

					//try
					//{
					//	db.SaveChanges();
					//}
					//catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
					//{
					//	Exception raise = dbEx;
					//	foreach (var validationErrors in dbEx.EntityValidationErrors)
					//	{
					//		foreach (var validationError in validationErrors.ValidationErrors)
					//		{
					//			string message = string.Format("{0}:{1}",
					//				validationErrors.Entry.Entity.ToString(),
					//				validationError.ErrorMessage);
					//			// raise a new exception nesting
					//			// the current instance as InnerException
					//			raise = new InvalidOperationException(message, raise);
					//		}
					//	}
					//	throw raise;
					//}

				}

				return View();
			}

			[HttpPost]
			public ActionResult AddData(CustomerViewModel model)
			{
				using (var db = new MvcCRUDDBEntities())
				{
					Customer data = new Customer();
					data.CustomerName = model.CustomerName;
					data.CustomerEmail = model.CustomerEmail;
					data.CustomerPhone = model.CustomerPhone;
					data.CustomerPassword = model.CustomerPassword;
					data.CustomerAddress = model.CustomerAddress;
					db.Customer.Add(data);
					db.SaveChanges();
				}
				return View("Index");
			}
	}
}