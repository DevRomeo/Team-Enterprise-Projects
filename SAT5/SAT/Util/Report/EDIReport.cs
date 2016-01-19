using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Dapper;
using SAT.Models;
using SAT.Enum;
using MySql.Data.MySqlClient;
using System.IO;
using OfficeOpenXml.Style;
using OfficeOpenXml;
namespace SAT.Util.Report
{
    public static class EDIReport
    {
        public static Byte[] export(List<EDIReportCAD> data, String loop, String start, String end, String report)
        {
            //  string report="";
            

                ExcelPackage p = new ExcelPackage();
                p.Workbook.Properties.Author = Environment.UserName;
                p.Workbook.Properties.Title = loop + " EDI Reports";
                p.Workbook.Properties.Company = "ML";

                p.Workbook.Worksheets.Add("EDI_" + report);
                ExcelWorksheet ws = p.Workbook.Worksheets[1];
                ws.Name = "EDI_" + report;
                ws.Column(1).Width = 15;
                ws.Column(2).Width = 25;
                ws.Column(3).Width = 50;
                ws.Column(4).Width = 25;

                ws.Cells[1, 1, 1, 3].Merge = true;
                ws.Cells[1, 1].Value = report + " Monthly Reports (" + loop + ") FROM : " + start + " TO : " + end;

                ws.Cells[2, 1].Value = "BOS Code";
                ws.Cells[2, 2].Value = "Branch Name";
                ws.Cells[2, 3].Value = "Total Share (100%) in PHP";


                ws.Cells[1, 1, 2, 4].Style.Font.Bold = true;
                ws.Cells[1, 1, 2, 4].Style.Font.Size = 10;
                int row = 3;
                for (int i = 0; i < data.Count; i++)
                {
                    ws.Cells[row, 1].Value = data[i].boscode.Trim();
                    ws.Cells[row, 2].Value = data[i].bosbranchname.Trim();
                    ws.Cells[row, 3].Value = data[i].one + data[i].two;
                    ws.Cells[row, 3].Style.Numberformat.Format = "#,###.00";

                    row++;
                }

                ws.Cells[row, 1, row, 3].Style.Font.Size = 10;
                ws.Cells[row, 1, row, 3].Style.Font.Name = "sansserif";
                Byte[] bin = p.GetAsByteArray();
                return bin;
        }
    }
}