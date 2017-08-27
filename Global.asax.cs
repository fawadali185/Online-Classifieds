using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Listhell.CODE;
using System.Security.Claims;
using System.Data.Entity.Validation;

namespace ListHell
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ValueProviderFactories.Factories.Add(new Listhell.CODE.CustomValueProviderFactory());
            System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
           
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            // Process 404 HTTP errors
            var httpException = exception as HttpException;
            //internet now working throwin exception
            LH_newEntities context = new LH_newEntities();
            ListHell.log l = new ListHell.log();
            l.error = exception.Message;
            l.source = exception.StackTrace;
            context.logs.Add(l);
            try { context.SaveChanges(); }
            catch (DbEntityValidationException e11)
            {
                foreach (var eve in e11.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        //save in text file
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            var e1 = Server.GetLastError();
            if (exception !=null)
            {
                Response.Clear();
                Server.ClearError();
                Response.TrySkipIisCustomErrors = true;

                IController controller = new ListHell.Controllers.ErrorController();

                var routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Ops");

                var requestContext = new RequestContext(
                     new HttpContextWrapper(Context), routeData);
                controller.Execute(requestContext);
            }
        }
    }
}
