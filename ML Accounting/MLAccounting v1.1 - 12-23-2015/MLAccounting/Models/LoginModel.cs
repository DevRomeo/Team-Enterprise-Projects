using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AccountingWeb.Models
{
    public class LoginModel
    {

        [Display(Name="User name")]
        [Required(ErrorMessage="Username is required")]
        public string Username { get; set; }
        [Display(Name = "Password ")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}