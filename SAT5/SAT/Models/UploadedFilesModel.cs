using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SAT.Models
{
    public class UploadedFilesModel
    {
    
        [DisplayName("Filename")]
        public String fname { get; set; }
        [DisplayName("Record Count")]
        public Int64 ftotal { get; set; }
        [DisplayName("Upload Date")]
        public DateTime SYSCREATED { get; set; }
        
        public int SYSCREATOR { get; set; }

        public int Status { get; set; }
        public String error { get; set; }
    }
}