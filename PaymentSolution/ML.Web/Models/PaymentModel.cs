using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ML.Web.Models
{
    public class PaymentModel
    {

        [Display(Name = "List of Group")]
        public string selectedgroup { get; set; }
        public IEnumerable<SelectListItem> group { get; set; }
        [Display(Name = "Total Remittance")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public decimal? remitance { get; set; }
        [Display(Name = "Total Charge")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public decimal? charge { get; set; }
        [Display(Name = "Grand Total")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public decimal? total { get; set; }
        [Display(Name = "Total Employee")]
        public int noofemployee { get; set; }

        [Display(Name = "ML Wallet Balance : ")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public decimal? Balance { get; set; }
    }
}