using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ML.Web.Models
{
    public class CancellationReportModel
    {
        [Display(Name = "Cancellation Time")]
        public string CancelTime { get; set; }
        [Display(Name = "Invoice No.")]
        public string InvoiceNo { get; set; }
        [Display(Name = "KTPN")]
        public string KPTN { get; set; }
        [Display(Name = "Group")]
        public string Group { get; set; }
        [Display(Name = "Receiver Name")]
        public string Employee { get; set; }
        [Display(Name = "Principal")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d+.\d{0,2}$")]
        public decimal Principal { get; set; }
        [Display(Name = "Charge")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d+.\d{0,2}$")]
        public decimal Charge { get; set; }
        [Display(Name = "Cancellation Charge")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d+.\d{0,2}$")]
        public decimal OtherCharge { get; set; }
        [Display(Name = "Resource")]
        public string CancelledBy { get; set; }
        public int index { get; set; }
    }
}