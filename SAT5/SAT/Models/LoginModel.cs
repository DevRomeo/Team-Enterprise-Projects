using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAT.Util;
using System.ComponentModel.DataAnnotations;
namespace SAT.Models
{
    public class LoginModel
    {
        public int id { get; set; }
        [Required(ErrorMessage="Pls input username")]
        public String userName { get; set; }
        [Required(ErrorMessage="Pls input password")]
        public String password { get; set; }

    }
}