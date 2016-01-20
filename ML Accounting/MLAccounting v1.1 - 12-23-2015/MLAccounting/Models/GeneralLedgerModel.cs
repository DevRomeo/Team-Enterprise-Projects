using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AccountingWeb.Models
{
    public class GeneralLedgerModel
    {
        [Display(Name = "GL")]                
        [RegularExpression(@"^[0-9\b]$", ErrorMessage = "GL Code accept only numbers.")]
        public string GeneralLedger { get; set; }
        [Display(Name = "GL Description")]
        [RegularExpression(@"^[a-zA-Z0-9 \/\-+&.%\b]+$", ErrorMessage = "GL Description field accept only letters.")]
        public string Description { get; set; }
        public string Category { get; set; }
        public bool choice { get; set; }
    }
}