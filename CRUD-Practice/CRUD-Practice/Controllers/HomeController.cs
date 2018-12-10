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
		// returns model for rendering all members data 
		public ActionResult Index()
		{
			var model = new CustomerViewModel();
			model.lstCustomer = model.GetCustomerList();
			return View(model);
		}


		public ViewResult Edit(decimal id)
		{
			using (var db=new MvcCRUDDBEntities1())
			{
				var customerData = db.Customers.Where(a => a.CustomerId == id).FirstOrDefault();
				// if id is the primary key in the database them directly write id
				// var customerData = db.Customers.Find(id);
				return View(customerData);
			}			
		}

		[HttpPost]
		public ActionResult Edit(CustomerViewModel model)
		{			
			var result = model.EditCustomer(model);
			return RedirectToAction("Index");
		}
	}
}