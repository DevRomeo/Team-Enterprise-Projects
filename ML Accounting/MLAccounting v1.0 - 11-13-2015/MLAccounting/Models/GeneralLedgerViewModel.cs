using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AccountingWeb.Models
{
    public class GeneralLedgerViewModel
    {
        public GeneralLedgerModel GeneralLedger { get; set; }
        public List<GeneralLedgerModel> GeneralLedgers { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public string Category { get; set; }

    }
}