using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Code_First_MVC.Models
{
    public class EmployeeContext:DbContext
    {
        public EmployeeContext():base("conn")
        {

        }

        public DbSet<Employee> Employees { get; set; }
    }
}