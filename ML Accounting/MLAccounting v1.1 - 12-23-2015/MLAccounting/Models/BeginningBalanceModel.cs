using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLAccounting.Models
{
    public class BeginningBalanceModel
    {
        public IEnumerable<SelectListItem> Branches
        {
            get
            {
                return new[]
                {
                    new SelectListItem{ Value="", Text =""}
                };
            }
        }
        public IEnumerable<SelectListItem> CorpName
        {
            get
            {
                return new[] 
                {
                    new SelectListItem{ Value="", Text=""}                    
                };
            }
        }


        public string Zone { get; set; }
        [Display(Name = "Corporate Name : ")]
        [Required(ErrorMessage = "Please select corporate name")]
        public string CorporateName { get; set; }
        [Display(Name = "Branch : ")]
        [Required(ErrorMessage = "Please select Branch")]
        public string Branch { get; set; }
        public string BranchCode { get; set; }
        public string GLNumber { get; set; }
        [Display(Name = "GL Account : ")]
        [Required(ErrorMessage = "Please provide GL Account")]
        public string GLDescription { get; set; }
        [Display(Name = "Amount : ")]
        [Required(ErrorMessage = "Please supply beginning balance")]        
        [RegularExpression(@"^(?=.*\d)\d*(?:\.\d\d)?$", ErrorMessage = "Invalid Beginning Balance Format")]
        [StringLength(12, ErrorMessage = "Maximum digit")]
        public string Amount { get; set; }
    }
}