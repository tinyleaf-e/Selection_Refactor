using System.Web;
using System.Web.Mvc;
using Selection_Refactor.Attribute;

namespace Selection_Refactor
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new FilterExceptionAttribute());
        }
    }
}
