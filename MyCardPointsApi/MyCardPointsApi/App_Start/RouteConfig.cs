using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyCardPointsApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{MyCardNo}/{GameServiceId}",
                defaults: new { controller = "api", action = "MyCardQuery", MyCardNo = UrlParameter.Optional, GameServiceId = UrlParameter.Optional }
            );
        }
    }
}