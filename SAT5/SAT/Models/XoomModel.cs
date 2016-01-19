using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAT.Models
{
    public class XoomModel
    {
        public string fname { get; set; }
        public string txndate { get; set; }
        public string Event_Date { get; set; }
        public string Created { get; set; }
        public string Event_Type { get; set; }
        public string Disbursement_Type { get; set; }
        public string Partner { get; set; }
        public string blank1 { get; set; }
        public string blank2 { get; set; }
        public string blank3 { get; set; }
        public string Branch_Number { get; set; }
        public string Authorizer { get; set; }
        public string Country { get; set; }
        public string Recipient_City { get; set; }
        public string Xoom_Invoice { get; set; }
        public string Payout_Currency { get; set; }
        public decimal Payout_Amount { get; set; }
        public string Payout_Currency1 { get; set; }
        public decimal Payout_Amount1 { get; set; }
        public double rate { get; set; }
        public double convertedusdamt { get; set; }
        public double charge { get; set; }
    }
}