using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAT.Models
{
    public class EDIReportCAD
    {
        public String txndate { get; set; }
        public String Branch_Number { get; set; }
        public String Event_Date { get; set; }
        public String Event_Type { get; set; }
        public Double Payout_Amount { get; set; }
        public String Payout_Currency { get; set; }
        public String boscode { get; set; }
        public String branchnumber { get; set; }
        public String bosbranchname { get; set; }
        public String specificregion { get; set; }
        public Double one { get; set; }
        public Double two { get; set; }
    }
}