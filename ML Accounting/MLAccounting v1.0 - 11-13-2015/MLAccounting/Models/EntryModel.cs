using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AccountingWeb.Models
{
    public class EntryModel
    {

        [Display(Name = "GL")]
        public string GL { get; set; }
        [Display(Name = "Account Description")]
        public string Description { get; set; }
        [Display(Name = "Debit")]
        public decimal Debit { get; set; }
        [Display(Name = "Credit")]
        public decimal Credit { get; set; }
        [Display(Name = "Reference")]
        [StringLength(50)]
        public string Reference { get; set; }
        [Display(Name = "Description")]
        [StringLength(50)]
        public string Remarks { get; set; }
        public bool Add { get; set; }
        public int EntryId { get; set; }
    }
}