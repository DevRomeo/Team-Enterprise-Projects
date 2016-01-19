using System;

namespace Comparative_Report.DataClass
{
    class D365Data
    {
        public int day { get; set; }
        public int year { get; set; }
        public Int16 mo { get; set; }
        public Int64 txtcount { get; set; }
        public Decimal principal { get; set; }
        public Decimal charge { get; set; }
        public String partnercode { get; set; }
        public String partnername { get; set; }
        public Int16 int_type { get; set; }
        public String currency { get; set; }
    }
}
