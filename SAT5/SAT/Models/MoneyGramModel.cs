using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAT.Models
{
    public class MoneyGramModel
    {
        public string fname { get; set; }
        public string trandate { get; set; }
        public string refnum { get; set; }
        public string trantype { get; set; }
        public string sendcntry { get; set; }
        public string rcvcntry { get; set; }
        public string legacyid { get; set; }
        public string agentname { get; set; }
        public string currency { get; set; }
        public decimal fxratephp { get; set; }
        public decimal margin { get; set; }
        public decimal amount { get; set; }
        public decimal feeamount { get; set; }
        public decimal shareamount { get; set; }
        public decimal commandfxamount { get; set; }
        public double rate { get; set; }
        public decimal convertedcommfxamt { get; set; }
    }
}