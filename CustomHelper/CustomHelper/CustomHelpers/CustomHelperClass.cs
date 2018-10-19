using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomHelper.CustomHelpers
{
    public static class CustomHelperClass
    {
        public static MvcHtmlString Submitbtn(this HtmlHelper mvc, string type, string val)
        {
            string str = "<input type='" + type + "' value='" + val + "'>";
            return new MvcHtmlString(str);
        }
    }
}