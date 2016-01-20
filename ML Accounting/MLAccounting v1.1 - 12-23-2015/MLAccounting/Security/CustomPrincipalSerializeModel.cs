using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLAccountingWeb.Security
{
    public class CustomPrincipalSerializeModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string[] roles { get; set; }
        public string task { get; set; }
    }
}