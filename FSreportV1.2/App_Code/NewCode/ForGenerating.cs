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
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;  

/// <summary>
/// Summary description for ForGenerating
/// </summary>
public class ForGenerating
{
    private ConnectionFileServer confs = new ConnectionFileServer();
    public string downloadPDF(string foldername,string filename)
    {
        string returnVal = string.Empty;
        var x = confs.MapFileServer();

        if (x.respcode == 1)
        {
            returnVal = dlfromWeb(foldername, confs.directory, filename);
        }
        else
        {
            return returnVal = x.msg;
        }

        return returnVal;
    }
    public void downloadFileAllFolder(string filename)
    {
         var x = confs.MapFileServer();

        if (x.respcode == 1)
        {
            string[] filepath = Directory.GetDirectories(confs.directory);
            foreach (string val in filepath)
            {
                string file = val + "\\" + filename;
                if (File.Exists(file))
                {
                    string dl = dlfromWeb(val.Remove(0, confs.directory.Length), confs.directory, filename);
                }
            }
        }

    }

    public string dlfromWeb(string foldername, string directory,string file)
    {
        //System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(directory );

        //FileInfo[] fi =dir.GetFiles("*" + date + ".pdf");

        string filename = directory + "\\" + foldername + "\\" + file;
        if (File.Exists(filename))
        {
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "pdf";
            response.AddHeader("Content-Disposition", "attachment; filename =" + filename + ";");
            response.TransmitFile(filename);
            response.Flush();
            response.End();
            return "";
        }
        else
        {
            return "No Files Found"; //directory + "\\" + filename;
        }
    }

    public List<string> allFolders()
    {
        List<string> ls = new List<string>();
        var x = confs.MapFileServer();

        if (x.respcode == 1)
        {
            string[] filepath = Directory.GetDirectories(confs.directory);
            foreach (string val in filepath)
            {
                ls.Add(val.Remove(0,confs.directory.Length));
            }
        }
        else
        {
            ls = null;
        }
        return ls;
    }

    public List<ListValue> allFiles(string foldername,string date)
    {
        List<ListValue> ls = new List<ListValue>();
        var x = confs.MapFileServer();

        if (x.respcode == 1)
        {
            string dir = confs.directory + "\\" + foldername;
            string[] filepath = Directory.GetFiles(dir, "*.pdf");
            foreach (string val in filepath)
            {
                if (val.Contains(date))
                {
                    ListValue lv = new ListValue();
                    lv.pdfFileName = val.Remove(0, dir.Length).Replace(@"\", "");

                    ls.Add(lv);
                }
            }
        }
        else
        {
            ls = null;
        }
        return ls;
    }
 //*********************************ALL checkbox is enable true********************************   
    public List<ListValue> allFilesInFolder()
    {
        var x = confs.MapFileServer();
        List<ListValue> ls = new List<ListValue>();
        if (x.respcode == 1)
        {
            string dir = confs.directory;
            string[] filepath = Directory.GetDirectories(dir);
            foreach (string val in filepath)
            {
                string[] filepath1 = Directory.GetFiles(val, "*.pdf");
                foreach (string val1 in filepath1)
                {
                   ListValue lv = new ListValue();
                   lv.pdfFileName = val1.Remove(0, val.Length).Replace(@"\", "");
                   ls.Add(lv);
                }
            }
        }
        else
        {
            ls = null;
        }

        return ls;
    }
    public List<ListValue> allFilesMonthly(string foldername, string date)
    {
        List<ListValue> ls = new List<ListValue>();
        var x = confs.MapFileServer();

        if (x.respcode == 1)
        {
            string dir = confs.directory + "\\" + foldername;
            string[] filepath = Directory.GetFiles(dir, "*.pdf");

            foreach (string val in filepath) 
            {
                if (Regex.IsMatch(val, date.Split('-')[0] + "-*-" + date.Split('-')[2]))
                {
                    ListValue lv = new ListValue();
                    lv.pdfFileName = val.Remove(0, dir.Length).Replace(@"\", "");

                    ls.Add(lv);
                }
            }
            
            //from xx in filepath where 
            //ls = filepath.Where(xx => Regex.IsMatch(xx,date.Split('-')[0] + "-*-" + date.Split('-')[2])).ToList();

        }
        else
        {
            ls = null;
        }
        return ls;
    }
    
}


   
