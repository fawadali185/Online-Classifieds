using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ListHell.CODE
{
    public class CustomFilter: System.Web.Mvc.ActionFilterAttribute
    {
      public   bool _f = true;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(filterContext.HttpContext.Request.QueryString["c"]==null && (string) filterContext.RouteData.Values["action"]=="area" && _f ==true && filterContext.HttpContext.Request.Cookies["lid"]!=null && filterContext.HttpContext.Request.Cookies["area"] != null )
            {
                _f = false;

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "lh" }, { "action", "categories" }, { "lid",filterContext.HttpContext.Request.Cookies["lid"].Value }, { "isPost", filterContext.HttpContext.Request.QueryString["isPost"] } });
            }
            
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _f = true;
        }
    }
}