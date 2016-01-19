using System;
using MySql.Data.MySqlClient;
namespace Comparative_Report.Util
{
    class Connection
    {
        
        //private String username;
        //private String password;
        //private String server;
        //private String dbname;
        //private Int32 timeout;
      
         private String inipath;
         private INI inifile;
        public Connection()
        {
            inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            inifile = new INI(inipath);
        }
        public MySqlConnection getPartnersConnection() 
        {
            String inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            INI inifile = new INI(inipath);
      
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = inifile.IniReadValue("Partners", "Server");
            builder.UserID = inifile.IniReadValue("Partners", "UserId"); ;
            builder.Password = inifile.IniReadValue("Partners", "Password");
            builder.Database = inifile.IniReadValue("Partners", "Database");

            return new MySqlConnection(builder.ToString());
        }
        public MySqlConnection getFileUploadConnection()
        {
            String inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            INI inifile = new INI(inipath);

            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = inifile.IniReadValue("FileUpload", "Server");
            builder.UserID = inifile.IniReadValue("FileUpload", "UserId"); ;
            builder.Password = inifile.IniReadValue("FileUpload", "Password");
            builder.Database = inifile.IniReadValue("FileUpload", "Database");

            return new MySqlConnection(builder.ToString());
        }
        public MySqlConnection getBillsPayConnection()
        {
            String inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            INI inifile = new INI(inipath);

            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = inifile.IniReadValue("BillsPay", "Server");
            builder.UserID = inifile.IniReadValue("BillsPay", "UserId"); ;
            builder.Password = inifile.IniReadValue("BillsPay", "Password");
            builder.Database = inifile.IniReadValue("BillsPay", "Database");

            return new MySqlConnection(builder.ToString());
        }
        public MySqlConnection getConnection(String DBConfig)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = inifile.IniReadValue(DBConfig, "Server");
            builder.UserID = inifile.IniReadValue(DBConfig, "UserId"); ;
            builder.Password = inifile.IniReadValue(DBConfig, "Password");
            builder.Database = inifile.IniReadValue(DBConfig, "Database");
            return new MySqlConnection(builder.ToString());
        }
    }

}
