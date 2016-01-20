using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccountingWeb.Models;

namespace MLAccountingWeb.Models
{
    public class UnproccessEntryModel
    {
        public EntryDetail EntryDetail { get; set; }

        public List<EntryDetail> EntryDetails { get; set; }
    }
}