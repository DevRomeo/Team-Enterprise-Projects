using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace EDIAgent.util
{
    class DBConnections
    {
        private string ediusername;
        private string edipassword;
        private string ediserver;
        private string edidb;

        private string synergyusername;
        private string synergypassword;
        private string synergyserver;
        private string synergyDb;
        //private string dbname;

        public DBConnections() 
        {
            string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            INI ini = new INI(inipath);
            ediusername = ini.IniReadValue("DBConfig EDI", "Username"); 
            edipassword = ini.IniReadValue("DBConfig EDI", "Password"); 
            ediserver = ini.IniReadValue("DBConfig EDI", "Server");
            edidb = ini.IniReadValue("DBConfig EDI", "DBName");

            synergyusername = ini.IniReadValue("DBConfig SYNERGY", "Username");
            synergypassword = ini.IniReadValue("DBConfig SYNERGY", "Password");
            synergyserver = ini.IniReadValue("DBConfig SYNERGY", "Server");
            synergyDb = ini.IniReadValue("DBConfig SYNERGY", "DBName");

            //dbname = ini.IniReadValue("DBConfig server", "Dbname"); 
      
        }
        public SqlConnection getEDIConnection() 
        {
            return new SqlConnection("server=" + ediserver + ";User Id=" + ediusername + ";password=" + edipassword + ";database="+edidb);
        }
        public SqlConnection getSynergyConnection()
        {
            return new SqlConnection("server=" + synergyserver + ";User Id=" + synergyusername + ";password=" + synergypassword + ";database="+synergyDb);
        }
    }
}
