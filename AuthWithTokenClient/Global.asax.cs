using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AuthWithTokenClient
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        ///     Minden Request küldés előtt lefutó metódus.
        /// </summary>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            /// Clickjacking elleni védelem bekapcsolása
            HttpContext.Current.Response.AddHeader("X-Frame-Options", "DENY");

            /// Expires (Lejárati idő) beállítása
            HttpContext.Current.Response.Cache.SetExpires(new DateTime(2000, 1, 1, 1, 0, 0));
        }
    }
}
