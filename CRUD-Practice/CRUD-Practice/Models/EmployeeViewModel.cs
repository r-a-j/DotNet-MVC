using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUD_Practice.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Office { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<int> Salary { get; set; }
    }
}