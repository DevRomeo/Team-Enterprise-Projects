using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ML.Web.Models
{
    public class CancelReportModel
    {
        public string CencelledTime { get; set; }
        public string Invoice { get; set; }
        public string KPTN { get; set; }
        public string Group { get; set; }
        public string Receiver { get; set; }
        public decimal Principal { get; set; }
        public decimal Charge { get; set; }
        public decimal OtherCharge { get; set; }
        public string Employer { get; set; }
    }
}