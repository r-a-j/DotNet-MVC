//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRUD_Practice.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
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
