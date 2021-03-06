﻿using System;
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


		public Nullable<System.DateTime> AddedDateTime { get; set; }
		public Nullable<decimal> AddedBy { get; set; }
		public Nullable<System.DateTime> UpdatedDateTime { get; set; }
		public Nullable<decimal> UpdatedBy { get; set; }
		public Nullable<decimal> DeletedBy { get; set; }
		public Nullable<System.DateTime> DeletedDateTime { get; set; }
		public Nullable<bool> IsDelete { get; set; }



		public List<CustomerViewModel> GetCustomerList()
		{
			using (var db = new MvcCRUDDBEntities1())
			{
				// converting all the data into list datatype
				// getting the data from the data base 
				//var customerData = db.Customers.ToList();

				//Used stored procedure for the displaying the data 
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
				var customerData = db.tblCustomer.Find(model.CustomerId);				
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
				var customerData = db.tblCustomer.Where(a => a.CustomerId == id).FirstOrDefault();
				db.tblCustomer.Remove(customerData);
				return db.SaveChanges() > 0;
			}
		}

		public CustomerViewModel getCustomerByID(decimal? id)
		{
			using (var db=new MvcCRUDDBEntities1())
			{
				var customerData = db.tblCustomer.Where(a => a.CustomerId == id).FirstOrDefault();

				CustomerViewModel model = new CustomerViewModel();
				model.CustomerId = customerData.CustomerId;
				model.CustomerEmail = customerData.CustomerEmail;
				model.CustomerAddress = customerData.CustomerAddress;
				model.CustomerName = customerData.CustomerName;
				return model;
			}
		}
	}
}