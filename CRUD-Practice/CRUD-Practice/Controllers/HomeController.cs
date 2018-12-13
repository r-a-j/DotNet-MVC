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
			using (var db = new MvcCRUDDBEntities1())
			{
				var customerData = db.tblCustomer.Where(a => a.CustomerId == id).FirstOrDefault();
				// if id is the primary key in the database then directly write id

				// This selects all the data of particular di but is slower
				// var customerData = db.Customers.Find(id);

				// Manually giving the data to view model.
				// This is faster
				CustomerViewModel model = new CustomerViewModel();
				model.CustomerId = customerData.CustomerId;
				model.CustomerEmail = customerData.CustomerEmail;
				model.CustomerAddress = customerData.CustomerAddress;
				model.CustomerName = customerData.CustomerName;
				return View(model);
			}
		}

		[HttpPost]
		public ActionResult Edit(CustomerViewModel model)
		{
			var result = model.EditCustomer(model);
			return RedirectToAction("Index");
		}

		public ActionResult DeleteCustomer(decimal id)
		{
			CustomerViewModel model = new CustomerViewModel();

			// getting the bool result from the DeleteCustomer method from CustomerViewModel class
			// checking if data is deleted or not
			if (model.DeleteCustomer(id))
				return RedirectToAction("Index");
			else
				return null;
		}

		[HttpPost]
		public ActionResult AddCustomer(CustomerViewModel model)
		{
			using (var db = new MvcCRUDDBEntities1())
			{
				tblCustomer data = new tblCustomer();
				if (model.CustomerId > 0)
				{

					var result = model.EditCustomer(model);
					

					return Json(new { msg = "success", JsonRequestBehavior.AllowGet });
				}
				else
				{
					data.CustomerName = model.CustomerName;
					data.CustomerEmail = model.CustomerEmail;
					data.CustomerAddress = model.CustomerAddress;
					db.tblCustomer.Add(data);
					db.SaveChanges();
				}
				
				
			}

			return PartialView("_AddEditCustomer", model);
			
		}

		[HttpGet]
		public ActionResult openCustomer(decimal? id)
		{
			CustomerViewModel data = new CustomerViewModel();

			if (id > 0)
			{
				data = data.getCustomerByID(id);
			}

			return PartialView("_AddEditCustomer", data);

		}
	}
}