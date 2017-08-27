using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ListHell.Areas.Admin.Controllers
{
    public class lhController : Controller
    {
        // GET: Admin/lh
        public ActionResult Index()
        {
            return View();
        }
    }
}