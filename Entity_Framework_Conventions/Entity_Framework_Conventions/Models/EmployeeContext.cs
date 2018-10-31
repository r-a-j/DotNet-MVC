using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Entity_Framework_Conventions.Models
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext() : base("conn") { }        
        public DbSet<Employee> Employees { get; set; }

    }
}