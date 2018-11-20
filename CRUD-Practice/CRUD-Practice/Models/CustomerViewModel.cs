using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUD_Practice.Models
{
    public class CustomerViewModel
    {
        public Nullable<int> CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public int CustomerPhone { get; set; }
        public string CustomerPassword { get; set; }
        public string CustomerAddress { get; set; }
    }
}