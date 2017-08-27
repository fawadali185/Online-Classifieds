using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ListHell.Models
{
    public class CustomAuthorizeAttribute: AuthorizeAttribute
    {
       public override void OnAuthorization(AuthorizationContext filterContext)
        {


            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // auth failed, redirect to login page
                filterContext.Result = new HttpUnauthorizedResult();
            }
            IPrincipal currentPrincipal = Thread.CurrentPrincipal;
            base.OnAuthorization(filterContext);
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Controller.TempData["ErrorDetails"] = "You must be logged in to access this page";
                filterContext.Result = new RedirectResult("~/User/Login");
                return;
            }
            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Controller.TempData["ErrorDetails"] = "You don't have access rights to this page";
                filterContext.Result = new RedirectResult("~/User/Login");
                return;
            }
        }

    }
}