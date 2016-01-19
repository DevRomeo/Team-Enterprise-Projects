using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparative_Report.DataClass
{
    class DSummary_data
    {
        public String partnername { get; set; }
        public String partnercode { get; set; }
        public Decimal txncurrent { get; set; }
        public Decimal txnprev { get; set; }
        public String intType { get; set; }
        public double var { get; set; }
        public double per { get; set; }

    }
    class DSummary_data2 : DSummary_data 
    {
        public Decimal princurrent { get; set; }
        public Decimal prinprev { get; set; }
        public Decimal chargecurrent { get; set; }
        public decimal chargeprev { get; set; }
    }
}
