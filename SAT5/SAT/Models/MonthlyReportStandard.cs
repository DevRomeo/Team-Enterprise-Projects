using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAT.Models
{
    public class MonthlyReportStandard
    {
        public String boscode { get; set; }
        public String bosbranchname { get; set; }
        public Double poamtphp { get; set; }
        public Double poamtusd { get; set; }
        public int pocntphp { get; set; }
        public int pocntusd { get; set; }
        public Double share { get; set; }
        public String specificregion { get; set; }
    }
}