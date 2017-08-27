using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ListHell.CODE
{
    using System.Collections.Generic;

    public partial class Categories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Categories()
        {
            this.ads = new HashSet<ads>();
        }

        
        public int id { get; set; }
        public string category1 { get; set; }
        public string master_category { get; set; }
        public string category_descr { get; set; }
        public string master_category_descr { get; set; }
        public string editedby { get; set; }
        public Nullable<System.DateTime> editedon { get; set; }
        public string master_category_icon { get; set; }
        public Nullable<int> lid { get; set; }
        public int? count { get; set; }

        public virtual Location Locations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ads> ads { get; set; }

    }
}