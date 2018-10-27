using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataAnnotations_and_Validations.Models
{
    public class Employee
    {
        [Required(ErrorMessage ="Please enter the username")]
        [StringLength(30)]
        [Display(Name ="Username")]
        public string username { get; set; }

        [Required(ErrorMessage = "Please enter the email")]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please enter an proper email")]
        [Display(Name = "E-mail")]
        public string email { get; set; }

        [Required(ErrorMessage = "Please enter the Age")]
        [Range(25, 30)]
        [Display(Name = "Age")]
        public int age { get; set; }

        [Required(ErrorMessage = "Please enter the Password")]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required(ErrorMessage = "Please enter the Password Again")]
        [Display(Name = "Re-Password")]
        [Compare("password", ErrorMessage ="Please Enter the same password")]
        public string repassword { get; set; }

        [Required(ErrorMessage ="Please accept the terms and conditions ")]
        [validateCheckBox(ErrorMessage ="Please Accept Terms & Conditions")]
        [Display(Name ="Accept terms and conditions")]
        public bool Terms { get; set; }

        public class validateCheckBox : RequiredAttribute
        {
            public override bool IsValid(object value)
            {
                return (bool)value;
            }
        }

    }
}