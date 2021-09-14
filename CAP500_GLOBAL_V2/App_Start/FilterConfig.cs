using System.Web;
using System.Web.Mvc;

namespace CAP500_GLOBAL_V2space
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}