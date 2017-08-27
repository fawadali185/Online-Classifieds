using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ListHell.Controllers
{
    public class lhController : Controller
    {

        CODE.DAL dal;
        public lhController()
        {
            dal = new CODE.DAL();

        }
        // GET: lh
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> area()
        {
            
            List<CODE.Area> v = await dal.getAreas();
            return View(v);
        }
        public async Task<ActionResult> cities(string area)
        {
            List<CODE.Locations> v = await dal.getCities(area);
            return View(v);
        }

        public async Task<ActionResult> categories(int? lid)
        {
            if (lid == null) { return RedirectToAction("area"); }
            List<CODE.Categories> v = await dal.getCategories(lid);
            List < CODE.Area > area= await dal.getAreas();

            ViewBag.area = area;
            ViewBag.g = await dal.GetG(lid);
            return View(v);

        }

     
        public async Task<string> getCitiesForArea(string area)
        {
            List<CODE.Locations> v = await dal.getCities(area);
            var javaScriptSerializer = new
    System.Web.Script.Serialization.JavaScriptSerializer();
            string data1 = javaScriptSerializer.Serialize(v);
            
            return data1;
        }


        public async Task<ActionResult> ads(int lid=0, int catid=0, int pn = 0,string search="",string areastr="",string city="")
        {
            List<CODE.Area> area = await dal.getAreas();

            ViewBag.area = area;
            if (search != "") {
                int x = await dal.getCount(lid, catid,search);
                ViewBag.paging = x + "," + pn;
                List<CODE.AdsModel> v = await dal.getSearch(search, areastr, city);
                return View(v);

            } else { 
            List<CODE.AdsModel> v = await dal.getAds(lid, catid, pn);
            int x = await dal.getCount(lid, catid);
            ViewBag.paging = x + "," + pn;
            ViewBag.ct = catid.ToString();
            ViewBag.lid = lid.ToString();
                return View(v);
            }
            
        }
        public async Task<ActionResult> paging(int cp=1)
        {
            string str = Request.QueryString["cp"] == null ? (1).ToString() : (Request.QueryString["cp"].ToString());
            List<CODE.Categories> v =await dal.paging(Convert.ToInt32(str)-1);
          
            HttpCookie c = new HttpCookie("cp");
            c.Value = str;
            int x =await dal.getCount();
            Response.Cookies.Add(new HttpCookie("count",x.ToString()));
            
            Response.Cookies.Add(c);
            return View(v);
        }
        public async Task<ActionResult> getAutosDetails()
        {
            return PartialView("_Autos");
        }
        [HttpGet]
        public async Task<ActionResult> Post()
        {
            if (Request.QueryString["catm"].ToString() == "autos" || Request.QueryString["catm"].ToString() == "bs" || Request.QueryString["catm"].ToString() == "SUV")
            {
                ViewBag.amountText = "Price";
            }
            else
            {
                ViewBag.amountText = "Pay";
            }

            if (Request.QueryString["lid"] != null)
            {
                string lid = Request.QueryString["lid"].ToString();
                string str = await dal.getSymbol(lid);
                ViewBag.symbol = str;
            }
            else
            {
                string lid = Request.Cookies["lid"].Value.ToString();
                string str = await dal.getSymbol(lid);
                ViewBag.symbol = str;
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Post(CODE.PostModel model)
        {
            string username = "";
            if (User != null) { username = User.Identity.Name; }
            model.GetType().GetCustomAttributes(false);
            if (!ModelState.IsValid) { return View(); }
            else
            {
                List<string> f = await dal.PostAd(model, username);
                Session["lstImg"] = f;
                Session["PostModel"] = model;
            }
            return RedirectToAction("PostMessage");
        }

        public async Task<ActionResult> PostMessage()
        {
            CODE.PostModel pst = (CODE.PostModel) Session["PostModel"];
            ViewBag.PostMessage = "Your ad is Posted and appeard on ad listings";
            return View(pst);
        }
        public async Task<ActionResult> GetAdsByUsers()
        {

            string username = User.Identity.Name;
            List<CODE.AdsModel> lst = await dal.getUserAds(username);
            return View(lst);
        }

        public async Task<ActionResult> DeleteUserAd(int adid, int catid, int lid)
        {
            bool a = await dal.DeleteUserAd(adid, catid, lid, User.Identity.Name);
            if (a == true)
                return RedirectToAction("GetAdsByUsers", new { posted = "true" });

            else
                return RedirectToAction("GetAdsByUsers", new { posted = "false" });

        }

        public async Task<ActionResult> getUserAdDetails(int adid, int catid, int lid)
        {
            ListHell.CODE.UserDetailedAd details = await dal.getUserAdDetails(adid, catid, lid, User.Identity.Name);
            return View(details);

        }

        public async Task<ActionResult> removeImage(int adid, int catid, int lid, string imgid)
        {
            bool v = await dal.removeImage(adid, Convert.ToInt32(imgid));
            if (v == true)
                return RedirectToAction("getUserAdDetails", new { adid = adid, catid = catid, lid = lid, os = "true" });
            else
                return RedirectToAction("getUserAdDetails", new { adid = adid, catid = catid, lid = lid, os = "false" });
        }

        public async Task<ActionResult> updateAd() {
            var ad = await dal.updateAd0(Request.QueryString["adid"].ToString(), Request.QueryString["catid"].ToString(), Request.QueryString["lid"].ToString());
            return View(ad); }

        [HttpPost]
        public async Task<ActionResult> updateAd(CODE.AdsModel obj)
        {
            bool v= await dal.updateAd(obj);
            if (v == true)
                return RedirectToAction("GetAdsByUsers",new { e="true"});
            else
                return RedirectToAction("GetAdsByUsers",new { e="false"});
        }
        public async Task<ActionResult> addetails(int adid, int catid, int lid)
        {
            List<CODE.Area> area = await dal.getAreas();

            ViewBag.area = area;
            ListHell.CODE.UserDetailedAd details = await dal.addetails(adid, catid, lid);
            return View(details);


        }

    }

}