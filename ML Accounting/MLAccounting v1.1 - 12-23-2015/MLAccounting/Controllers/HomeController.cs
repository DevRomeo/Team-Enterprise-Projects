using AccountingWeb.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using MLAccountingWeb.Models;
using MLAccountingWeb.Controllers;
using MLAccountingWeb.Security;
using ML.AccountingSystemV1.Contract.Parameters;
using ML.AccountingSystemV1.Service;
using ML.AccountingSystemV1.Service.Class;
using ML.AccountingSystemV1.Contract;

namespace AccountingWeb.Controllers
{
    //[CustomAuthorize(Roles="Admin")]    
    //[CustomAuthorize(Roles="BM,Admin")]
    [HandleError(View = "UnexpectedError")]
    [Authorize]    
    public class HomeController : BaseController
    {
        private readonly iAccountingService service;
        public HomeController(iAccountingService service)
        {
            this.service = service;
        }
        [Route("Home")]        
        [OutputCache(Duration = 10)]        
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View("Index");
            }
            return RedirectToAction("_UNAuthorizedUser", "Errors");
        }

        [Route("Entries")]
        [HttpGet]
        [OutputCache(Duration = 5)]
        public ActionResult UnprocessEntries(string dt)
        {
            //AccountingService srvc = new AccountingService();
            var resp = this.service.UnprocessEntries(dt);
            UnproccessEntryModel unprocessEntry = new UnproccessEntryModel();
            if (resp.resp == 1)
            {
                unprocessEntry.EntryDetails = (from m in resp.data
                                               select new EntryDetail
                                               {
                                                   CorpName = m.corpname,
                                                   EntryNo = m.EntryNo,
                                                   TotalCredit = m.totalCredit * -1,
                                                   TotalDebit = m.totalDebit
                                               }).ToList();
            }
            else
            {
                unprocessEntry.EntryDetails = null;
            }

            return PartialView("_partialUnprocessEntries", unprocessEntry);
        }

        [HttpPost]
        [Route("Unprocess/Submit")]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessEntry(UnproccessEntryModel unprocess)
        {
            if (ModelState.IsValid && unprocess.EntryDetails.Count(m => m.Process == true) > 0)
            {
                //AccountingService srvc = new AccountingService();
                List<ParamProcess> forProcessing = (from m in unprocess.EntryDetails.Where(n => n.Process == true)
                                                    select new ParamProcess
                                                    {
                                                        EntryNo = m.EntryNo,
                                                        Corpname = m.CorpName
                                                    }).ToList();

                var rsponse = this.service.ProcessEntry(forProcessing);
                if (rsponse.respcode == 1)
                {

                    return Json(new { code = rsponse.respcode, msg = rsponse.message, action = Url.Action("Index", "Home") });
                }
                else
                {
                    return Json(new { msg = rsponse.message });
                }


            }
            return Json(new { msg = "Please select Data Entry to be process" });
        }

    }
}