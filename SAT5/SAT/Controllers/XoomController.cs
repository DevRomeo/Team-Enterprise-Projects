using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAT.Models;
using SAT.Enum;
using SAT.Util;

namespace SAT.Controllers
{
    [Authorize]
    public class XoomController : Controller
    {
        // GET: Xoom
        public ActionResult Index()
        {
            var data = XoomUtil.getAllUploadedFile();
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
                response = XoomUtil.isUploaded(file.FileName);
                if (response.responseCode == ResponseCode.OK)
                {
                    response = XoomUtil.readCsv(file);
                    if (response.responseCode == ResponseCode.OK)
                    {
                        List<XoomModel> data = (List<XoomModel>)response.responseData;
                        response = XoomUtil.insertToXoom(data, file.FileName);


                    }

                }

            }
            return Json(response);
        }

      
    }
}
