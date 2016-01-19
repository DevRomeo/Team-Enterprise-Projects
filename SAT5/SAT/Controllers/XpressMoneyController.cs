using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAT.Models;
using SAT.Enum;
using SAT.Util;
using SAT.Util.Report;
using System.IO.Compression;
using System.IO;
using Ionic.Zip;


namespace SAT.Controllers
{
    [Authorize]
    public class XpressMoneyController : Controller
    {
        // GET: XpressMoney
        public FileResult test() 
        {
            var outputStream = new MemoryStream();
            //List<MonthlyReportStandard> temp10
            List<MonthlyReportStandard> temp = new List<MonthlyReportStandard>();
            //List<MonthlyReportStandard> temp1 = new List<MonthlyReportStandard>();

            //List<MonthlyReportStandard> temp2 = new List<MonthlyReportStandard>();
            Byte [] data1 =ReportMontly.createExcelBytes(temp, "test", DateTime.Now, DateTime.Now.AddDays(1));
            Byte[] data2 = ReportMontly.createExcelBytes(temp, "test", DateTime.Now, DateTime.Now.AddDays(1));
            Byte[] data3 = ReportMontly.createExcelBytes(temp, "test", DateTime.Now, DateTime.Now.AddDays(1));
  //          GZipStream 

            var zip = new ZipFile();
            zip.AddEntry("file1.xlsx", data1);
            zip.AddEntry("file2.xlsx", data2);
            zip.AddEntry("file3.xlsx", data3);
            zip.Save(outputStream);
            //var testt = new Byte[2]();
//            GZipStream zipStream = new GZipStream(;
            outputStream.Position = 0;
            return File(outputStream, "application/zip", "filename.zip");
            //return File(ReportMontly.createExcelBytes(temp, "test", DateTime.Now, DateTime.Now.AddDays(1)),
              //  System.Net.Mime.MediaTypeNames.Application.Octet,"test.xlsx");
        }
        public ActionResult Index()
        {
            var data = XpressMoneyUtil.getAllUploadedFile();
            List<UploadedFilesModel> files = (List<UploadedFilesModel>)(data.responseData);
            if (files == null)
            {
                files = new List<UploadedFilesModel>();
            }
            return View(files);
        }
        [HttpPost]
        public ActionResult getSheets(HttpPostedFileBase file) 
        {
            return Json(XpressMoneyUtil.getSheets(file));
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, String sheetNo)
        {
            CustomResponse response = new CustomResponse();
            response =XpressMoneyUtil.isUploaded(file.FileName);
           

            if (response.responseCode != ResponseCode.OK)
            {
                return Json(response);

            }
            else
            {
                response =XpressMoneyUtil.readSheet(file, sheetNo);
                if (response.responseCode == ResponseCode.OK)
                {
                    response =XpressMoneyUtil.insetToXpressmoney((List<XpressMoneyModel>)response.responseData, sheetNo);
                }
              
            }
          
            return Json(response);
        }

        
    }
}
