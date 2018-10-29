using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Code_First_MVC.Models
{
    public class Employee
    {
        [Key]
        public int empid { get; set; }
        public int empname { get; set; }
        public int address { get; set; }
    }
}