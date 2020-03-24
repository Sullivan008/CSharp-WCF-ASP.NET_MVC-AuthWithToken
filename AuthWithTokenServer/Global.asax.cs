using System;
using System.Web;

namespace AuthWithTokenServer
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        { }

        protected void Session_Start(object sender, EventArgs e)
        { }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, authorization, token");

            HttpContext.Current.Response.AddHeader("x-frame-options", "DENY");

            HttpContext.Current.Response.Cache.SetExpires(new DateTime(2000, 1, 1, 1, 0, 0));

            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");

                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, authorization, token");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");

                HttpContext.Current.Response.AddHeader("x-frame-options", "DENY");

                HttpContext.Current.Response.Cache.SetExpires(new DateTime(2000, 1, 1, 1, 0, 0));

                HttpContext.Current.Response.End();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        { }

        protected void Application_Error(object sender, EventArgs e)
        { }

        protected void Session_End(object sender, EventArgs e)
        { }

        protected void Application_End(object sender, EventArgs e)
        { }
    }
}