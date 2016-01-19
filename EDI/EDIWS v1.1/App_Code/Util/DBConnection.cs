using System;
using System.Data.SqlClient;
using log4net;
/// <summary>
/// Summary description for DBConnection
/// </summary>
public class DBConnection
{
    public DBConnection()
    { }
   
    public SqlConnection getEDIConnection(String edi) 
    {
        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        try 
        { 
            INI iniFile = new INI(inipath);
            String username = iniFile.IniReadValue(edi, "Username");
            String password = iniFile.IniReadValue(edi, "Password");
            String server = iniFile.IniReadValue(edi, "Server");
            String dbname = iniFile.IniReadValue(edi, "DBName");
            return new SqlConnection("server=" + server + ";User Id=" + username + ";password=" + password + ";database=" + dbname);
        }
        catch (Exception e ) 
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error(edi + " " + e.Message, e);
            return null; 
        }
    }
    public SqlConnection getMaintenanceConnection()
    {
        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        try
        {
            INI iniFile = new INI(inipath);
            String username = iniFile.IniReadValue("Maintenance", "Username");
            String password = iniFile.IniReadValue("Maintenance", "Password");
            String server = iniFile.IniReadValue("Maintenance", "Server");
            String dbname = iniFile.IniReadValue("Maintenance", "DBName");
            return new SqlConnection("server=" + server + ";User Id=" + username + ";password=" + password + ";database=" + dbname);
        }
        catch (Exception e)
        {
            Service.log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Service.log.Error("Maintenance" + " " + e.Message, e);
            return null;
        }
    }


}