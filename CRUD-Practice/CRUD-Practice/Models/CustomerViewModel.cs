using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRUD_Practice.Models
{
	public class CustomerViewModel
	{
		public decimal CustomerId { get; set; }
		public string CustomerName { get; set; }
		public string CustomerEmail { get; set; }
		public decimal CustomerPhone { get; set; }
		public string CustomerPassword { get; set; }
		public string CustomerRePassword { get; set; }
		public string CustomerAddress { get; set; }

		public List<CustomerViewModel> lstCustomer { get; set; }

		public List<CustomerViewModel> GetCustomerList()
		{
			using (var db = new MvcCRUDDBEntities1())
			{
				// converting all the data into list datatype
				//var customerData = db.Customers.ToList();
				var customerData = db.USP_GetCustomerList().ToList();
				// for storing all data in list
				var list = new List<CustomerViewModel>();

				if (customerData.Count > 0)
				{
					// storing each member data seperatly in lst object
					foreach (var item in customerData)
					{
						var lst = new CustomerViewModel();
						lst.CustomerId = item.CustomerId;
						lst.CustomerName = item.CustomerName;
						lst.CustomerEmail = item.CustomerEmail;
						lst.CustomerAddress = item.CustomerAddress;
						list.Add(lst);
					}
				}
				return list;
			}
		}

		public bool EditCustomer(CustomerViewModel model)
		{
			using (var db = new MvcCRUDDBEntities1())
			{
				var customerData = db.Customers.Find(model.CustomerId);
				customerData.CustomerName = model.CustomerName;
				customerData.CustomerAddress = model.CustomerAddress;
				customerData.CustomerEmail = model.CustomerEmail;
				db.SaveChanges();
				return true;
			}
		}

		public bool DeleteCustomer(decimal id)
		{
			using (var db = new MvcCRUDDBEntities1())
			{
				var customerData = db.Customers.Where(a => a.CustomerId == id).FirstOrDefault();
				db.Customers.Remove(customerData);
				return db.SaveChanges() > 0;
			}
		}
	}
}