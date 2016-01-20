using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ML.Web.Models
{
    public class CacenllationReportModel
    {
        public string Date { get; set; }
        public string Employer { get; set; }
        public string Group { get; set; }
        public List<CancelReportModel> data { get; set; }
    }
}