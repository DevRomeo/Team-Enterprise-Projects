using AccountingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accounting.Models
{
    public class GLReportModel
    {
        public string date { get; set; }
        public string Remarks { get; set; }
        public string Reference { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal NetChange { get; set; }
        public int isLastDayOfMonth { get; set; }
        public decimal BeginningBalance { get; set; }
    }
    public class GLReportViewModel
    {
        public GeneralLedgerModel GeneralLedger { get; set; }
        public List<GLReportModel> GLEntrys { get; set; }
        public decimal GLBeginningBalance { get; set; }
        public string Corporate { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string periodFrom { get; set; }
        public string periodTo { get; set; }

    }
}