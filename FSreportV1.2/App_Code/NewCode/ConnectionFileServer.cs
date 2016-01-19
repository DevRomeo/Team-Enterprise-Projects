using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
//using data = System.Int;

/// <summary>
/// Summary description for ConnectionFileServer
/// </summary>
public class ConnectionFileServer
{
    private MapDrive.Network.MapDrive mapdriver = new MapDrive.Network.MapDrive();
    public string directory;
    private string pathFileServer = AppDomain.CurrentDomain.BaseDirectory + "ExecReport.ini";
    public string mapserver;
    public string mapuser;
    public string mappass;

    public Response MapFileServer()
    {
        
        int i = 0;
        IniFile ini = new IniFile(pathFileServer);
        directory = ini.IniReadValue("PDF Directory", "dir");
        mapserver = ini.IniReadValue("PDF Directory", "mapdrive");
        mapuser = ini.IniReadValue("PDF Directory", "mapuser");
        mappass = ini.IniReadValue("PDF Directory", "mappass");
        string mapletter = ini.IniReadValue("PDF Directory", "mapletter");

        if (Directory.Exists(directory) != true)
        {
            bool res = mapdriver.MapNetworkDrive(mapserver, Convert.ToChar(mapletter), true, mapuser, mappass);
            if (res == true)
            {
                return new Response { respcode = 1, msg = "Connecting Successfully" };
            }
            else
            {
                return new Response { respcode = 0, msg = "Failed to connect file Server" };
            }
        }
        else
        {
            return new Response { respcode = 1, msg = "Connected" };
        }
    }

    public void MapFileServerfunct()
    {

        int i = 0;
        IniFile ini = new IniFile(pathFileServer);
        directory = ini.IniReadValue("PDF Directory", "dir");
        mapserver = ini.IniReadValue("PDF Directory", "mapdrive");
        mapuser = ini.IniReadValue("PDF Directory", "mapuser");
        mappass = ini.IniReadValue("PDF Directory", "mappass");
        string mapletter = ini.IniReadValue("PDF Directory", "mapletter");

    }
}
