using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SAT.Models
{
    public class MonthlyReportRequestModel
    {
        
        
        [Required(ErrorMessage = "Please input valid year")]
        public int year { get; set; }
        [Required(ErrorMessage = "Please input valid Month")]
        [Range(1,12, ErrorMessage = "Please input valid Month")]
        public int month { get; set; }
        public String report { get; set; }
    }
}