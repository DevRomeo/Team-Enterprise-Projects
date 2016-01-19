using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace SAT.Models
{
    public class EDIRequestReportModel:MonthlyReportRequestModel
    {
        [Required(ErrorMessage="Please select a valid report to generate")]
        
        public String Zone { get; set; }

        [Required(ErrorMessage = "Please select a valid report type to generate")]
        [Display(Name="Report Format")]
        public String Type { get; set; }

    }
}