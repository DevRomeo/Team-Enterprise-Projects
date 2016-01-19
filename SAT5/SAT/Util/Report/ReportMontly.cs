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
    public static class ReportMontly
    {
        public static Byte[] createExcelBytes(List<MonthlyReportStandard> data, String report, DateTime start, DateTime end)
        {

          
            ExcelPackage p = new ExcelPackage();
            p.Workbook.Properties.Author = Environment.UserName;
            p.Workbook.Properties.Title = report + " Monthly Reports";
            p.Workbook.Properties.Company = "ML";

            p.Workbook.Worksheets.Add(report);
            ExcelWorksheet ws = p.Workbook.Worksheets[1];
            ws.Name = "report";
            ws.Column(1).Width = 15;
            ws.Column(2).Width = 25;
            ws.Column(3).Width = 25;
            ws.Column(4).Width = 25;
            ws.Column(5).Width = 15;
            ws.Column(6).Width = 15;
            ws.Column(7).Width = 25;

            // ws.Cells[1, 1, 1, 3].Merge = true;
            ws.Cells[1, 1].Value = "Volume of Transactions for " + report;
            ws.Cells[2, 1].Value = "Year";
            ws.Cells[2, 2].Value = start.ToString("MMM") + start.ToString("yyyy");
            ws.Cells[2, 3].Value = "Amount";
            ws.Cells[2, 5].Value = "Transaction Count";
            ws.Cells[2, 7].Value = "Total Share (100%) in PHP";
            ws.Cells[2, 7].Style.WrapText = true;

            ws.Cells[2, 1, 3, 1].Merge = true;
            ws.Cells[2, 2, 3, 2].Merge = true;

            ws.Cells[1, 1, 1, 7].Merge = true;
            ws.Cells[2, 3, 3, 4].Merge = true;

            ws.Cells[2, 5, 3, 6].Merge = true;
            ws.Cells[2, 7, 3, 7].Merge = true;

            ws.Cells[4, 1].Value = "Date Generated";
            ws.Cells[4, 2].Value = DateTime.Now.ToString();
            ws.Cells[4, 3].Value = "PHP";
            ws.Cells[4, 4].Value = "USD";
            ws.Cells[4, 5].Value = "PHP";
            ws.Cells[4, 6].Value = "USD";


            ws.Cells[1, 1, 4, 7].Style.Font.Bold = true;
            ws.Cells[1, 1, 4, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[1, 1, 4, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[1, 1].Style.Font.Size = 14;
            ws.Cells[2, 1, 4, 7].Style.Font.Size = 12;
            int row = 6;
            String[] regions = data
                    .GroupBy(X => X.specificregion)
                   .Select(grp => grp.First().specificregion)
                   .ToArray();
            List<MonthlyReportStandard> ddata = new List<MonthlyReportStandard>();
            for (int i = 0; i < regions.Length; i++)
            {
                String region = regions[i];
                ddata = data.Where(x => x.specificregion == region).ToList();
                ws.Cells[row, 1].Value = region;
                ws.Cells[row, 1].Style.Font.Bold = true;
                ws.Cells[row, 1].Style.Font.Size = 12;
                row++;
                int startRow = row;
                for (int j = 0; j < ddata.Count; j++)
                {
                    ws.Cells[row, 1].Value = ddata[j].boscode;
                    ws.Cells[row, 2].Value = ddata[j].bosbranchname;
                    ws.Cells[row, 3].Value = ddata[j].poamtphp;
                    ws.Cells[row, 4].Value = ddata[j].poamtusd;
                    ws.Cells[row, 5].Value = ddata[j].pocntphp;
                    ws.Cells[row, 6].Value = ddata[j].pocntusd;
                    ws.Cells[row, 7].Value = ddata[j].share;

                    ws.Cells[row, 3, row, 4].Style.Numberformat.Format = "#,###.00";
                    ws.Cells[row++, 7].Style.Numberformat.Format = "#,###.00";
                }
                ws.Cells[row, 1].Value = "Subtotal";
                ws.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[row, 3].Formula = "sum(C" + startRow.ToString() + ":C" + (row - 1).ToString() + ")";
                ws.Cells[row, 4].Formula = "sum(D" + startRow.ToString() + ":D" + (row - 1).ToString() + ")";
                ws.Cells[row, 5].Formula = "sum(E" + startRow.ToString() + ":E" + (row - 1).ToString() + ")";
                ws.Cells[row, 6].Formula = "sum(F" + startRow.ToString() + ":F" + (row - 1).ToString() + ")";
                ws.Cells[row, 7].Formula = "sum(G" + startRow.ToString() + ":G" + (row - 1).ToString() + ")";

                ws.Cells[row, 1, row, 2].Merge = true;
                ws.Cells[row, 1, row, 7].Style.Font.Size = 12;
                ws.Cells[row, 1, row, 7].Style.Font.Bold = true;
                ws.Cells[row, 3, row, 4].Style.Numberformat.Format = "#,###.00";
                ws.Cells[row++, 7].Style.Numberformat.Format = "#,###.00";
            }

            Double poamtphp = (from s in data
                               select s.poamtphp).Sum();
            Double poamtusd = (from s in data
                               select s.poamtusd).Sum();
            Double pocntphp = (from s in data
                               select s.pocntphp).Sum();
            Double pocntusd = (from s in data
                               select s.pocntusd).Sum();
            Double share = (from s in data
                            select s.share).Sum();


            ws.Cells[row, 1].Value = "GrandTotal";
            ws.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws.Cells[row, 3].Value = poamtphp;
            ws.Cells[row, 4].Value = poamtusd;
            ws.Cells[row, 5].Value = pocntphp;
            ws.Cells[row, 6].Value = pocntusd;
            ws.Cells[row, 7].Value = share;

            ws.Cells[row, 1, row, 2].Merge = true;
            ws.Cells[row, 1, row, 7].Style.Font.Size = 12;
            ws.Cells[row, 1, row, 7].Style.Font.Bold = true;
            ws.Cells[row, 3, row, 4].Style.Numberformat.Format = "#,###.00";
            ws.Cells[row, 5, row, 6].Style.Numberformat.Format = "#,###";
            ws.Cells[row, 7].Style.Numberformat.Format = "#,###.00";

                Byte[] bin = p.GetAsByteArray();
                return bin;
        
        }
    }
}