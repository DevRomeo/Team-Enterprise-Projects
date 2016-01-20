using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountingWeb.Models
{
    public class EntryDetail
    {
        public int EntryId { get; set; }
        [Display(Name = "Entry No.")]
        public string EntryNo { get; set; }

        [Display(Name = "Corporate Names")]
        public string CorpName { get; set; }
        [Display(Name = "Total Debit")]
        public decimal TotalDebit { get; set; }
        [Display(Name = "Total Credit")]
        public decimal TotalCredit { get; set; }
        
        [Display(Name="Process")]
        public bool Process { get; set; }
    }
}