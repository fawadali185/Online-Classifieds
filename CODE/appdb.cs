using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace ListHell.CODE
{
    public class appdb:DbContext
    {
        public appdb()
            : base("name=DefaultConnection1")
        {
        }

     
        public virtual DbSet<Models.test> tests { get; set; }
    }
}