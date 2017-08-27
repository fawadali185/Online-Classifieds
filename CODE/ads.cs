using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ListHell.CODE
{
    public partial class ads
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ads()
        {
            this.Images = new HashSet<Images>();
        }
        [Key]
        public int adid { get; set; }
      
        public int catid { get; set; }
       
        public int lid { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public Nullable<System.DateTime> datetime { get; set; }
        public Nullable<decimal> amount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Images> Images { get; set; }
        public virtual Category Categories { get; set; }
        public virtual Location Locations { get; set; }
    }
}