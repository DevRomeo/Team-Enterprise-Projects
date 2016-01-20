using AccountingWeb;
using AccountingWeb.Models;
using ML.AccountingSystemV1.Contract.Parameters;
using ML.AccountingSystemV1.Service;
using MLAccounting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ML.AccountingSystemV1.Contract;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace MLAccounting.Controllers
{
    [HandleError(View = "UnexpectedError")]
    [Authorize(Users = "AccountingAdmin")]
    public class AdminController : Controller
    {
        private readonly iAccountingService service;
        public AdminController(iAccountingService service)
        {
            this.service = service;
        }
        //
        // GET: /Admin/
        [Route("BeginningBalance")]
        public ActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //[Route("BeginningBalance")]
        //public ActionResult BeginningBalance()
        //{
        //    return View(new BeginningBalanceModel());
        //}        
        [OutputCache(Duration = 30)]
        [HttpGet]
        [Route("ini")]
        public ActionResult BeginningBalanceComponent()
        {
            //AccountingService srvc = new AccountingService();
            var dataEntryComponent = this.service.CorporanameReturnValue();

            List<GeneralLedgerModel> GeneralLedger = (from m in this.service.GlAccount_Description().data
                                                      select new GeneralLedgerModel
                                                      {
                                                          GeneralLedger = m.GLcode,
                                                          Description = m.Description
                                                      }).ToList();

            return Json(new
            {
                code = dataEntryComponent.respcode,
                account = PartialToString.RenderPartialViewToString(this, "_partialGL", GeneralLedger),
                component = dataEntryComponent.returnData.Where(m => m.Corpname != null && m.Corpname == "MICHEL J. LHUILLIER FINANCIAL SERVICES (PAWNSHOPS),INC." && m.branchName == "ML COLON 1"),
                corporatenames = dataEntryComponent.returnData.Where(m => m.Corpname != null && m.Corpname == "MICHEL J. LHUILLIER FINANCIAL SERVICES (PAWNSHOPS),INC.").Select(m => m.Corpname).Distinct().ToArray(),
                page = PartialToString.RenderPartialViewToString(this, "_BeginningBalance", new BeginningBalanceModel())
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("BeginningBalance")]
        [ValidateAntiForgeryToken]
        public ActionResult BeginningBalance(BeginningBalanceModel data)
        {
            if (ModelState.IsValid)
            {
                //AccountingService srvc = new AccountingService();
                var response = this.service.Add_BeginningBalance(new generalParam() { amount = Convert.ToDecimal(data.Amount), bcode = data.BranchCode, date = DateTime.Now.ToString("yyyy-MM-dd"), Glnumber = data.GLNumber, zcode = data.Zone });
                return Json(new { code = response.respcode, message = response.message });
            }
            return Json(new { code = 0, message = "Beginning Balance Form not valid" });
        }

        //[Route("Excel")]
        //public ActionResult Excel()
        //{
        //    GridView gv = new GridView();

        //    gv.DataSource = (from m in this.service.GlAccount_Description().data
        //                     select new
        //                     {
        //                         GeneralLedger = m.GLcode,
        //                         Description = m.Description
        //                     }).ToList();
        //    gv.DataBind();

        //    Response.ClearContent();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition", "attachment; filename=Marklist.xls");
        //    Response.ContentType = "application/ms-excel";
        //    Response.Charset = "";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter htw = new HtmlTextWriter(sw);
        //    gv.RenderControl(htw);
        //    Response.Output.Write(sw.ToString());
        //    Response.Flush();
        //    Response.End();

        //    return RedirectToAction("Index");
        //}
    }
}