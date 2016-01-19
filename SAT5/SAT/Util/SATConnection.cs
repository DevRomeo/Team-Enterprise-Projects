using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
namespace SAT.Util
{
    public static class SATConnection
    {
        public static MySqlConnection getMoneygramConnection()
        {
            return new MySqlConnection(ConfigurationManager.ConnectionStrings["moneygram"].ToString());
        }
        public static MySqlConnection getXoomConnection()
        {
            return new MySqlConnection(ConfigurationManager.ConnectionStrings["Xoom"].ToString());
        }
        public static MySqlConnection getXpressMoneyConnection()
        {
            return new MySqlConnection(ConfigurationManager.ConnectionStrings["xpressmoney"].ToString());
        }
        public static MySqlConnection getSAT5Connection()
        {
            return new MySqlConnection(ConfigurationManager.ConnectionStrings["sat5"].ToString());
        }
    }
}