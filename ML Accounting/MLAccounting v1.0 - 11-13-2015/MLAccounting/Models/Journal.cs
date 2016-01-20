using AccountingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounting.Models
{
    public class Journal : EntryModel
    {
        public String EntryDate { get; set; }
    }

    public class JournalViewModel
    {
        public int BatchNo { get; set; }
        public string EntryDate { get; set; }
        public string CreationDate { get; set; }
        public string Corporate { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public Dictionary<int, List<Journal>> Journals { get; set; }
    }

}