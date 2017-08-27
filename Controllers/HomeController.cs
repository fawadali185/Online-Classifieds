using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ListHell.Controllers
{
    public class HomeController : Controller
    {
        
      
        public async Task<ActionResult> Index()
        {
           // CODE.DAL dal = new CODE.DAL();
         //bool f=  await dal.flagit(2015);
           
            return View();
        }
        [Authorize(Roles ="auser")]
        public ActionResult About()
        {
            ViewBag.Message = "Listhell is a free classifieds website present in all countries.";

            return View();
        }

        [Models.CustomAuthorize(Roles ="auser")]
        public ActionResult Contact()
        {           
            ViewBag.Message = "Contact Details.";

            return View();
        }
    }
}