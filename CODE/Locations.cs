using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ListHell.CODE
{
    using System.Collections.Generic;

    public partial class Locations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Locations()
        {
            this.Categories = new HashSet<Categories>();
            this.ads = new HashSet<ads>();
        }

        public int id { get; set; }
        public string city { get; set; }
        public string state_province { get; set; }
        public string area { get; set; }
        public string description { get; set; }
        public string editedby { get; set; }
        public string editedon { get; set; }
        public string currency_symbol { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Categories> Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ads> ads { get; set; }
    }
}