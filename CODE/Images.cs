using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ListHell.CODE
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Images
    {
        [Key]
        public int imageid { get; set; }
      
        public int adid { get; set; }

        public string src { get; set; }
        public Nullable<bool> defaulti { get; set; }

        public virtual ads ad { get; set; }
    }
}