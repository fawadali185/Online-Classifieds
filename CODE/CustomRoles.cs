using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ListHell.CODE
{
    public class CustomRoles: IRole<int>
    {
        public CustomRoles()
        {
            
        }
        public  int Id { get; set; }
        public  ICollection<Users>  Users
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
    }
    
}