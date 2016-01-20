using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ML.Web.Models
{
    public class HomeViewModel
    {
        [DisplayName("PN #")]
        public string PN { get; set; }
        [DisplayName("Amount")]
        public string amount { get; set; }
        [DisplayName("No. of Employee")]
        public string NoEmp { get; set; }
        public string status { get; set; }
        public string groups { get; set; }
        public string processed { get; set; }
    }
}