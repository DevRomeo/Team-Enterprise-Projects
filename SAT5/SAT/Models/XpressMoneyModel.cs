using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAT.Models
{
    public class XpressMoneyModel
    {
        public string xpresscode { get; set; }
        public string branchname { get; set; }
        public string payoutdate { get; set; }
        public string currency { get; set; }
        public decimal amount { get; set; }
        public decimal refund { get; set; }
        public decimal tax { get; set; }
        public decimal comm { get; set; }
        public decimal amountpaid { get; set; }
        public string beneficiary { get; set; }
        public string xpn { get; set; }
        public string sendingagent { get; set; }
        public string userid { get; set; }
        public string fname { get; set; }
        public decimal fee { get; set; }
    }
}