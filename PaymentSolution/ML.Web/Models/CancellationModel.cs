using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ML.Web.Models
{
    public class CancellationModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Invoice No")]
        public string Invoice { get; set; }
        [Display(Name = "KPTN")]
        public string KPTN { get; set; }
        [Display(Name = "Group")]
        public string Group { get; set; }
        [Display(Name = "Salary")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d+.\d{0,2}$")]
        public decimal Amount { get; set; }
        public bool Option { get; set; }

        [Display(Name = "Reason for cancellation ?")]
        public string Reason { get; set; }
        public decimal CancellationCharge { get; set; }
    }
}