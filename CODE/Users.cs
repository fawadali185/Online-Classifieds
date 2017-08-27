using ListHell.CODE;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ListHell.CODE
{
    public partial class Users : IUser<string>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
           
        }
      
        public  string Id { get; set; }//email
      
        public string UserName { get; set; }
        public string Email { get; set; }

        public string address { get; set; }
        public string phone { get; set; }
        public string details { get; set; }
        public string password { get; set; }

        public ICollection<CustomRoles> Roles { get; set; }
        public string PasswordHash { get; set; }
    }
}