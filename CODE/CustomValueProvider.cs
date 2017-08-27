using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace Listhell.CODE
{
    public class CustomValueProvider : IValueProvider
    {
        public bool ContainsPrefix(string prefix)
        {
            return HttpContext.Current.Request.Cookies[prefix] != null || HttpContext.Current.Session[prefix] != null;

        }

        public ValueProviderResult GetValue(string key)
        {
            return new ValueProviderResult(HttpContext.Current.Request.Cookies[key].Value, HttpContext.Current.Request.Cookies[key].Value.ToString(), CultureInfo.CurrentCulture);
            //    if (key == "Id")
            //{
            //        return new ValueProviderResult(HttpContext.Current.Request.Cookies[key].Value, HttpContext.Current.Request.Cookies[key].Value.ToString(), CultureInfo.CurrentCulture);
            //    }
            //else
            //{
            //    return new ValueProviderResult(HttpContext.Current.Session[key], HttpContext.Current.Session[key].ToString(), CultureInfo.CurrentCulture);
            //}
        }

    }
}