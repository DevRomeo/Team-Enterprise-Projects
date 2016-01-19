using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAT.Util
{
    public static class MySession
    {
        public static int userID { get; set; }
        public static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


    }
}