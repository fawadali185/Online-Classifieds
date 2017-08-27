using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ListHell
{
    public class RouteConfig
    {
       //static bool  x = false;
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            
                routes.MapRoute(name: "location", url: "{area}/cities",
                    defaults: new { controller = "lh", action = "cities", id = UrlParameter.Optional }, namespaces:new[] { "ListHell.Controllers" });
                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "lh", action = "area", id = UrlParameter.Optional }, namespaces: new[] { "ListHell.Controllers" }

                );            
      
        }
    }
}
