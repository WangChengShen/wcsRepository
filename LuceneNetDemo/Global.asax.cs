using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LuceneNetDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //≥ı ºªØlog4net
            string path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "log4net.Config");
            var fi = new System.IO.FileInfo(path);
            log4net.Config.XmlConfigurator.Configure(fi); 

        }
    }
}
