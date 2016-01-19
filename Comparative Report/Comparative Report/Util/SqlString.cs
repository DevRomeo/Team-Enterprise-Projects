
using System;
namespace Comparative_Report.Util
{
    class SqlString
    {
        
        public String getSql(String Filename)
        {
            String sql = "";
            sql = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory+"\\Query\\" + Filename + ".txt");//inifile.IniReadValue(Config, "sql");
            return sql;

        }
    }
}

