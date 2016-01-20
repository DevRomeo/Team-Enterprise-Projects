using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ML.Web.Models
{
    public class ForgatPasswordModel
    {

        [Required(ErrorMessage = "Email Address is required when reseting the password")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }
    }
}