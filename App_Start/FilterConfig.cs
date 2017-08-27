
using System.Web.Mvc;

namespace ListHell
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {

filters.Add(new ListHell.CODE.CustomFilter());
filters.Add(new HandleErrorAttribute());

        }
    }
}
