using System;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GAIN
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log.Info("Application started at " + DateTime.Now.ToString());
        }

        protected void Application_End()
        {
            log.Info("Application ended at " + DateTime.Now.ToString());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            if (!(ex is ThreadAbortException))
            {
                log.Error(ex.Message, ex);
                Server.ClearError();
                Response.Redirect("/Error/GeneralError", true);
            }
        }
    }
}
