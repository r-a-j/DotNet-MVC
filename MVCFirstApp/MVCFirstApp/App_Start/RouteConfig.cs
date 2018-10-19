using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCFirstApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "Default3", url: "api2/{id}",
              defaults: new
              {
                  controller = "Process",
                  action = "Student",
                  id = UrlParameter.Optional
              },
              constraints: new
              {
                  id = @"\d+"
              });

            routes.MapRoute(
              name: "Default2", url: "api/",
              defaults: new
              {
                  controller = "Process",
                  action = "Student",
                  id = UrlParameter.Optional
              });
            routes.MapRoute(
               name: "Default", url: "{controller}/{action}/{id}",
               defaults: new
               {
                   controller = "Home",
                   action = "Index",
                   id = UrlParameter.Optional
               });


        }
    }
}
