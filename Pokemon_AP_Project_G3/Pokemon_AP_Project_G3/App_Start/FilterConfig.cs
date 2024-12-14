using System.Web;
using System.Web.Mvc;

namespace Pokemon_AP_Project_G3
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
