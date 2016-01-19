using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for Connection
/// </summary>
public class Connection
{
    private string pathFileServer = AppDomain.CurrentDomain.BaseDirectory + "ExecReport.ini";
	public Connection()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public Connection(String DBConfig) 
    {
        this.DBConfig = DBConfig;
    }
    public String DBConfig { get; set; }
    public SqlConnection getConnection(String DBConfig) 
    {
        IniFile ini = new IniFile(pathFileServer);
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.UserID = ini.IniReadValue(DBConfig, "USERNAME");
        builder.Password = ini.IniReadValue(DBConfig, "PASSWORD");
        builder.DataSource = ini.IniReadValue(DBConfig, "SERVER");
        builder.InitialCatalog = ini.IniReadValue(DBConfig, "DBNAME");

        return new SqlConnection(builder.ToString());
    }
    public SqlConnection getConnection()
    {
        IniFile ini = new IniFile(pathFileServer);
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.UserID = ini.IniReadValue(this.DBConfig, "USERNAME");
        builder.Password = ini.IniReadValue(this.DBConfig, "PASSWORD");
        builder.DataSource = ini.IniReadValue(this.DBConfig, "SERVER");
        builder.InitialCatalog = ini.IniReadValue(this.DBConfig, "DBNAME");

        return new SqlConnection(builder.ToString());
    }
}
