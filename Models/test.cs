using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ListHell.Models
{
    public class test
    {
        [Key]
        public int testid { get; set; }
        public string testname { get; set; }
    }
}