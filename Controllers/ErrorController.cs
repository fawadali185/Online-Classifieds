using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ListHell.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public string NotFound()
        {
            return "Ops. Page. Not. Found. :(";
        }

        public ActionResult Ops()
        {
            ViewBag.m= "Oh no! Something went ready wrong. Try again";
            return View();
        }
    }
}