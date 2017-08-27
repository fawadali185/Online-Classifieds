using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ListHell.CODE
{
    public class AdsModel
    {

        public int adid { get; set; }
        public int lid { get; set; }
        public int catid { get; set; }
        public string title { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string catname { get; set; }
        public string locname { get; set; }
        public string areaname { get; set; }
        public DateTime? dt { get; set; }
        public decimal? amount { get; set; }
        public string descr { get; set; }
        public decimal? imageid { get; set; }
        public string src { get; set; }
        public string cat { get; set; }
        public string mastercat { get; set; }
        public string catdescr { get; set; }



    }
}