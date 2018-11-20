using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUD_Practice.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string OrderDestination { get; set; }
        public string OrderName { get; set; }
        public int OrderQuantity { get; set; }
        public int OrderPrice { get; set; }
        public int OrderBillNumber { get; set; }
    }
}