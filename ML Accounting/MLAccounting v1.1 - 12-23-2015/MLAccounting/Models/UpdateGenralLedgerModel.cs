using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountingWeb.Models
{
    public class UpdateGenralLedgerModel
    {
        public List<GeneralLedgerModel> GeneralLedgers { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public string Category { get; set; }
    }
}