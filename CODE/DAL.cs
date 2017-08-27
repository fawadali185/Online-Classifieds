using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;

namespace ListHell.CODE
{
    public class DAL
    {
        LH_newEntities context;
        flagen context1;
        public DAL()
        {
            context = new LH_newEntities();
            context1 = new flagen();
        }
        public async Task<List<Area>> getAreas()
        {
            var v = await context.Locations.DistinctBy(w => w.area).Select(w => new Area { Areas = w.area }).ToListAsync();
            //var v = await context.Locations.Select(w => new Area { Areas = w.area }).ToListAsync();
            return v;

        }
        public async Task<List<Locations>> getCities(string area)
        {
            
            List<CODE.Locations> v = await context.Locations.Where(w => w.area == area)
                                    .Select(w => new CODE.Locations
                                    {
                                        area = w.area,
                                        city = w.city,
                                        id = w.id,
                                        description = context.ads.Where(x => x.lid == w.id).Count().ToString()
                                    })
                                    .OrderBy(w => w.city)
                                    .ToListAsync();
            return v;
        }

        public async Task<List<AdsModel>> getSearch(string search, string area, string city, int pn = 0)
        {
            List<CODE.AdsModel> ads = await context.ads.Join(context.Locations, a => a.lid, b => b.id, (a, b) => new { x = a, y = b })
                                          .Where(w => (w.y.city == city && w.x.description.Contains(search) && w.y.area == area)
                                          || (w.x.title.Contains(search) && w.y.city == city && w.y.area == area))
                                               .Select(a => new CODE.AdsModel
                                               {
                                                   adid = a.x.adid,
                                                   address = a.x.address,
                                                   amount = a.x.amount,
                                                   catid = a.x.catid,
                                                   dt = a.x.datetime,
                                                   descr = a.x.description,
                                                   title = a.x.title,
                                                   lid = a.x.lid,
                                                   phone = a.x.phone,
                                                   src = a.x.Images.Where(w => w.adid == a.x.adid && w.defaulti == true).Select(w => w.src).FirstOrDefault()
                                               }).OrderByDescending(w => w.dt).Skip(pn * 10).Take(10).ToListAsync();



            return ads;
        }
        public async Task<List<CODE.Categories>> getCategories(int? lid)
        {
        //    var x1 = context.Categories.GroupJoin(
        //                                              context.ads,
        //                                             cat => cat.id,
        //                                             ad => ad.catid,
        //                                             (cat, ad) => new { cats = cat, ads = ad })
        //                                        .SelectMany(
        //                                             a => a.ads.DefaultIfEmpty(),
        //                                             (a, y) => new { catss = a.cats, adss = y }).GroupBy(w => w.adss,ww=>new {  cat=ww.catss,count=ww.catss.ads.Count()}).Where(s=>s.FirstOrDefault().cat.lid==7);
        //    //.Select(w => new {aaa=w.count,CCC=w.C }).Where(dd=>dd.aaa.FirstOrDefault().lid==7);

         //  var x2 = context.Categories.Select(cat => new { cat = cat, c = cat.ads.Count() }).Where(w => w.cat.lid == 7);


            List<CODE.Categories> v =await context.Categories
                       .Where(category => category.lid == lid)
                       .GroupJoin(
                           context.ads
                           , category => category.id
                           , ad => ad.catid
                           , (c, a) => new
                           {
                               Category = c,
                               NumberOfAds = a.DefaultIfEmpty().Count(x => x != null)
                           }).OrderBy(m=>m.Category.master_category)
                           .Select(w=>new CODE.Categories {
                                category1=w.Category.category1,
                                id=w.Category.id,
                                master_category_icon=w.Category.master_category_icon,
                                master_category=w.Category.master_category,
                                master_category_descr=w.Category.master_category_descr,
                                count=w.NumberOfAds,
                                lid=w.Category.lid
                           }).ToListAsync();
            return v;
            //List<CODE.Categories> v = await context.Categories
            //                                       .Select(w => new 
            //{
            //    category1 = w.category1,
            //    id = w.id,
            //    master_category_icon = w.master_category_icon,
            //    master_category = w.master_category,
            //    master_category_descr = w.master_category_descr,
            //    lid = w.lid



            //}).Where(w => w.lid == lid)
            //.GroupJoin(context.ads, a => a.id, b => b.catid,
            //(a, b) => new  { 
            //                    cat=a,ad=b

            //                                }).SelectMany(ab=>ab.ad)

            //                        .Select(x => new CODE.Categories
            //                        {
            //                            category1 = x.Category.category1,
            //                            id = x.catid,
            //                            master_category_icon = x.Category.master_category_icon,
            //                            master_category = x.Category.master_category,
            //                            master_category_descr = x.Category.master_category_descr,
            //                            lid = x.lid


            //                        })
            //                                .GroupBy(n => n.id)
            //                                .Select(n => new CODE.Categories
            //                                {

            //                                    category1 = n.FirstOrDefault().category1,
            //                                    id = n.FirstOrDefault().id,
            //                                    master_category = n.FirstOrDefault().master_category,
            //                                    master_category_descr = n.FirstOrDefault().master_category_descr,
            //                                    master_category_icon = n.FirstOrDefault().master_category_icon,
            //                                    count = n.Count()
            //                                }).OrderBy(w => w.master_category)


            //                                .ToListAsync();



            //return v;

        }
        public async Task<int> getCount(int lid = 0, int catid = 0, string search = "", string area = "", string city = "")
        {
            if (search != "")
            {
                int v = await context.ads.Where(w => w.catid == catid && w.lid == lid)
                                     .CountAsync();
                return v;
            }
            else
            {
                int v = await context.ads.Join(context.Locations, a => a.lid, b => b.id, (a, b) => new { x = a, y = b })
                                               .Where((w => w.y.city == city || w.x.description.Contains(search) && w.y.area == area))
                                               .Select(a => new CODE.AdsModel
                                               {
                                                   adid = a.x.adid,
                                                   address = a.x.address,
                                                   amount = a.x.amount,
                                                   catid = a.x.catid,
                                                   dt = a.x.datetime,
                                                   descr = a.x.description,
                                                   src = a.x.Images.Where(w => w.adid == a.x.adid && w.defaulti == true).Select(w => w.src).FirstOrDefault()
                                               }).CountAsync();
                return v;
            }

        }
        public async Task<List<AdsModel>> getAds(int lid, int catid, int pn = 0)
        {
            var v = await context.ads.Select(w => new
            {
                a = w

            }).Join(context.Categories, a => a.a.catid, b => b.id, (a, b) => new
            { x = a, y = b }).Where(w => w.x.a.catid == catid)
              .Join(context.Locations, a => a.x.a.lid, b => b.id, (a, b) => new
              {
                  adid = a.x.a.adid,
                  address = a.x.a.address,
                  phone = a.x.a.phone,
                  title = a.x.a.title,
                  catname = b.city,
                  areaname = b.area,
                  catid = a.x.a.catid,
                  lid = a.x.a.lid,
                  locname = b.state_province,
                  dt = a.x.a.datetime,
                  amount = a.x.a.amount,
                  descr = a.x.a.description,
                  cat = a.y.category1,
                  mastercat = a.y.master_category,
                  catdescr = a.y.category_descr


              }).Where(w => w.lid == lid)
              .GroupJoin(context.Images, a => a.adid, b => b.adid, (a, b) => new
              {
                  x = a,
                  y = b.Where(f => f.defaulti == true)
              })
                .Select(w => new AdsModel
                {
                    adid = w.x.adid,
                    catid = w.x.catid,
                    amount = w.x.amount,
                    lid = w.x.lid,
                    title = w.x.title,
                    dt = w.x.dt,
                    src = w.y.FirstOrDefault().src,
                    imageid = w.y.FirstOrDefault().imageid,
                    cat = w.x.cat,
                    catdescr = w.x.catdescr,
                    mastercat = w.x.mastercat,
                    descr = w.x.descr,
                    phone = w.x.phone

                })

              .OrderByDescending(w => w.dt).Skip(pn * 10).Take(10).ToListAsync();
            return v;
        }

        public async Task<string> getSymbol(string lid)
        {
            int l = Convert.ToInt32(lid);
            var s = await context.Locations
                                 .Select(w => new
                                 { symbol = w.currency_symbol, lid = w.id })
                                                .Where(x => x.lid == l).Select(w => w.symbol)
                                                .FirstAsync();
            return s.ToString();
        }

        public async Task<List<string>> PostAd(PostModel model, string un = "")
        {
            ListHell.ad na = new ListHell.ad();
            if (un != "")
            {
                var user = await context.Users.Where(w => w.UserName == un).FirstOrDefaultAsync();
                na.Users.Add(user);
            }
            na.lid = model.lid;
            na.catid = model.catid;
            na.title = model.title;
            na.description = model.description;
            na.phone = model.phone;
            na.address = model.address;
            na.amount = model.amount;
            na.datetime = DateTime.Now;
            na.email = model.email;
            context.ads.Add(na);
            context.SaveChanges();
            int x = 0;
            ListHell.Image i = new ListHell.Image();
            List<string> lst = new List<string>();
            foreach (var o in model.images)
            {
                if (o != null)
                {
                    i.src = "../ai/" + "_" + generateName() + "_" + o.FileName;//change it with guide+jpg
                    lst.Add(i.src);
                    if (x == 0)
                    {
                        i.defaulti = true;

                    }
                    else
                    {
                        i.defaulti = false;
                    }
                    try
                    {
                        o.SaveAs(HttpContext.Current.Server.MapPath(i.src));
                    }
                    catch (Exception ex) { }
                    i.adid = na.adid;
                    na.Images.Add(i);
                    context.Images.Add(i);
                    context.SaveChanges();

                }

                x++;

            }


            return lst;
        }

        public async Task<List<AdsModel>> getUserAds(string username)
        {
            string id = await context.Users.Where(w => w.UserName == username).Select(w => w.Id).FirstOrDefaultAsync();

            List<AdsModel> lst = await context.ads.Where(w => w.Users.FirstOrDefault().Id == id)
                                                .Select(x => new AdsModel
                                                {
                                                    address = x.address,
                                                    amount = x.amount,
                                                    adid = x.adid,
                                                    catid = x.catid,
                                                    cat = x.Category.category1,
                                                    catname = x.Category.category1,
                                                    descr = x.description,
                                                    mastercat = x.Category.master_category,
                                                    src = x.Images.Where(w => w.adid == x.adid && w.defaulti == true).Select(w => w.src).FirstOrDefault(),
                                                    locname = x.Location.city,
                                                    lid = x.lid,
                                                    dt = x.datetime,
                                                    areaname = x.Location.area,
                                                    phone = x.phone,
                                                    title = x.title,


                                                }).ToListAsync();
            return lst;
        }

        public async Task<bool> DeleteUserAd(int adid, int catid, int lid, string UserName)
        {
            try
            {
                string id = await context.Users.Where(w => w.UserName == UserName).Select(w => w.Id).FirstOrDefaultAsync();
                ListHell.ad a = new ListHell.ad { adid = adid, catid = catid, lid = lid };
                a.Users.Add(new User { Id = id });
                List<ListHell.Image> i = await context.Images.Where(w => w.adid == adid).ToListAsync();
                a.Images = i;
                context.ads.Attach(a);
                context.ads.Remove(a);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex) { return false; }

        }

        public async Task<UserDetailedAd> getUserAdDetails(int adid, int catid, int lid, string UserName)
        {

            var v = await context.ads.Where(w =>
                                        w.Users.FirstOrDefault().UserName == UserName
                                        && w.adid == adid
                                        && w.catid == catid
                                        && w.lid == lid)
                                        .Select(w => new UserDetailedAd
                                        {
                                            adid = w.adid,
                                            catid = w.catid,
                                            lid = w.lid,
                                            userid = w.Users.FirstOrDefault().UserName,
                                            title = w.title,
                                            images = w.Images.Select(x => new ImagesDetail
                                            {
                                                adid = x.adid,
                                                defaulti = x.defaulti,
                                                imageid = x.imageid,
                                                src = x.src
                                            }).ToList()
                                            ,
                                            phone = w.phone,
                                            city = context.Locations.Where(s => s.id == w.lid).Select(a => a.city).FirstOrDefault(),
                                            address = w.address,
                                            description = w.description,
                                            amount = w.amount,
                                            dt = w.datetime,
                                            master_cat = context.Categories.Where(b => b.id == w.catid).Select(z => z.master_category).FirstOrDefault()


                                        })
                                        .FirstOrDefaultAsync();

            return v;
        }


        public async Task<UserDetailedAd> addetails(int adid, int catid, int lid)
        {

            var v = await context.ads.Where(w =>
                                        w.adid == adid
                                        && w.catid == catid
                                        && w.lid == lid)
                                        .Select(w => new UserDetailedAd
                                        {
                                            adid = w.adid,
                                            catid = w.catid,
                                            lid = w.lid,
                                            title = w.title,
                                            images = w.Images.Select(x => new ImagesDetail
                                            {
                                                adid = x.adid,
                                                defaulti = x.defaulti,
                                                imageid = x.imageid,
                                                src = x.src
                                            }).ToList()
                                            ,
                                            phone = w.phone,
                                            city = context.Locations.Where(s => s.id == w.lid).Select(a => a.city).FirstOrDefault(),
                                            address = w.address,
                                            description = w.description,
                                            amount = w.amount,
                                            email = w.email,
                                            dt = w.datetime,
                                            master_cat = context.Categories.Where(b => b.id == w.catid).Select(z => z.master_category).FirstOrDefault()


                                        })
                                        .FirstOrDefaultAsync();

            return v;
        }

        public async Task<bool> removeImage(int adid, int imgid)
        {

            ListHell.Image img = new ListHell.Image { adid = adid, imageid = imgid };
            context.Images.Attach(img);
            context.Images.Remove(img);
            context.SaveChanges();
            return true;

        }


        public async Task<AdsModel> updateAd0(string adid, string catid, string lid)
        {
            int i = Convert.ToInt32(lid);
            int a0 = Convert.ToInt32(adid);
            int c = Convert.ToInt32(catid);
            var v = await context.ads.Where(w => w.adid == a0 && w.catid == c && w.lid == i).Select(x => x).FirstOrDefaultAsync();
            AdsModel a = new AdsModel();
            a.amount = v.amount;
            a.address = v.address;
            a.adid = v.adid;
            a.catid = v.catid;
            a.lid = v.lid;
            a.phone = v.phone;
            a.title = v.title;
            a.descr = v.description;
            return a;
        }

        public async Task<bool> updateAd(AdsModel a)
        {
            var v = await context.ads.Where(w => w.adid == a.adid && w.catid == a.catid && w.lid == a.lid).Select(x => x).FirstOrDefaultAsync();
            v.title = a.title;
            v.description = a.descr;
            v.amount = a.amount;
            v.phone = a.phone;
            v.address = a.address;
            context.SaveChanges();
            return true;
        }

        public async Task<List<AdsModel>> GetG(int? lid)
        {
            var v = await context.ads.Where(w => w.lid == lid)
                .GroupBy(x => x.catid, (key, a) => a.OrderByDescending(z => z.datetime).FirstOrDefault())
                .Select(b => new AdsModel
                {
                    title = b.title,
                    amount = b.amount,
                    src = b.Images.Where(i => i.defaulti == true).Select(i => new { i.src }).FirstOrDefault().src,
                    adid = b.adid,
                    catid = b.catid,
                    lid = b.lid
                }).ToListAsync();
            return v;

        }

        public async Task<bool> flagit(int adid)
        {

            flag f = new flag { ads = new ListHell.ad { adid = adid } };
            context1.flags.Add(f);
            context1.SaveChanges();
            return true;



        }

        public async Task<List<CODE.Categories>> paging(int cp)
        {

            List<CODE.Categories> v = await context.Categories.Select(w => new CODE.Categories { category1 = w.category1 }).OrderBy(w => w.category1).Skip(cp* 10).Take(10).ToListAsync();
            return v;
        }

        public async Task<int> getCount() {
            int c =  context.Categories.Count();
            return c; }

        private string generateName()
        {
            Guid g = Guid.NewGuid();
            return g.ToString();
        }



    }

    public class UserDetailedAd
    {
        public int adid { get; set; }
        public int catid { get; set; }
        public int lid { get; set; }
        public string userid { get; set; }
        public ICollection<ImagesDetail> images { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public decimal? amount { get; set; }
        public DateTime? dt { get; set; }
        public string cat { get; set; }
        public string master_cat { get; set; }
        public string city { get; set; }
        public string locname { get; set; }


    }

    public partial class ImagesDetail
    {

        public decimal imageid { get; set; }

        public int adid { get; set; }

        public string src { get; set; }
        public Nullable<bool> defaulti { get; set; }

        public virtual ads ad { get; set; }
    }
}