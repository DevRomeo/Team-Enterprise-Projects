using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ML.Web.Models
{
    public class CacheManifestModel
    {
        public string AssemblyVersion { get; set; }
        public List<string> CacheCollection { get; set; }
    }
}