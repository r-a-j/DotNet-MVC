using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttributeRouting.Controllers
{
    [RoutePrefix("StudentData")]
    public class StudentController : Controller
    {

        // GET: Student
        [Route("AllStudents")]
        public ActionResult Index()
        {            
            return Content("All Students Information");
        }

        [Route("StudentByRoll/{rolln}")]
        public ActionResult StudentbyRollNo(int rolln)
        {
            if(rolln>0)
            {
                return Content("Valid Roll Number");
            }
            else
            {
                return Content("InValid Roll Number");
            }
        }
    }
}