using System.Web;
using System.Web.Mvc;

namespace DataAnnotations_and_Validations
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
