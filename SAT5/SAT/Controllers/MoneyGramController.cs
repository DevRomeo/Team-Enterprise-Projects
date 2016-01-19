using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAT.Models;
using SAT.Util;
using SAT.Enum;
namespace SAT.Controllers
{
    [Authorize]
    public class MoneyGramController : Controller
    {
        // GET: MoneyGram
        public ActionResult Index()
        {
            var data = MoneyGramUtil.getAllUploadedFile();
            List<UploadedFilesModel> files = (List<UploadedFilesModel>)(data.responseData);
            if (files == null) 
            {
                files = new List<UploadedFilesModel>();
            }
            return View(files);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase file) 
        {

            CustomResponse response = new CustomResponse();
            if (file == null)
            {
                response.responseCode = ResponseCode.Error;
                response.responseMessage = "Invalid file";
            }
            else 
            {
                response = MoneyGramUtil.isUploaded(file.FileName);
                if(response.responseCode==ResponseCode.OK)
                {
                    response = MoneyGramUtil.readCsv(file);
                    if (response.responseCode == ResponseCode.OK)
                    {
                        List<MoneyGramModel> data = (List<MoneyGramModel>)response.responseData;
                        response =MoneyGramUtil.insertToMoneygram(data, file.FileName);
                     
                        
                    }
                    
                }
            
            }
            return Json(response);    
        }
       
    }
}
