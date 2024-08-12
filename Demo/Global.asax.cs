using Demo.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Demo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static bool s_InitializedAlready = false;
        private static Object s_lock = new Object();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GenerateCss.GenerateLessFile();
            GenerateCss.GenerateCssFile();
        }

        protected void Application_BeginRequest(object source, EventArgs e)
        {
            if (!s_InitializedAlready)
            {
                HttpApplication app = (HttpApplication)source;
                HttpContext context = app.Context;
                Initialize(context);
                var url = context.Request.Url;
                var domain = url.Host;
                BundleConfig.RegisterBundles(BundleTable.Bundles);
            }
        }

        private void Initialize(HttpContext context)
        {
            if (s_InitializedAlready)
            {
                return;
            }

            lock (s_lock)
            {
                if (s_InitializedAlready)
                {
                    return;
                }

                s_InitializedAlready = true;
            }
        }
    }
}
