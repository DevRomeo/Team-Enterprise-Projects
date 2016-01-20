using AccountingWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MLAccountingWeb.Security;
using MLAccountingWeb.Controllers;
using ML.AccountingSystemV1.Service.Class;
using ML.AccountingSystemV1.Service;

namespace AccountingWeb.Controllers
{
    //[CustomAuthorize(Roles = "Admin")]
    [Authorize(Users = "Administrator")]
    public class GLController : BaseController
    {
        [Route("GL/Register")]
        [HttpGet]
        [OutputCache(Duration = 5)]
        public ActionResult Add()
        {
            AccountingService srvc = new AccountingService();
            var srvcResponse = srvc.get_GLdata();
            GeneralLedgerViewModel glView = new GeneralLedgerViewModel();
            glView.GeneralLedgers = (from m in srvcResponse.glItems
                                     select new GeneralLedgerModel
                                     {
                                         GeneralLedger = m.GLcode,
                                         Description = m.Description
                                     }).ToList(); ;
            glView.Categories = (from m in srvcResponse.catItems select new SelectListItem { Value = m.Category_id.ToString(), Text = m.Description }).OrderBy(m => m.Text).ToList();
            return PartialView("_partialAddLedger", glView);
        }

        [Route("GL/Register")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(GeneralLedgerViewModel GL)
        {
            if (GL.GeneralLedgers.Where(m => m.choice == true).Count() > 0)
            {
                var gls = (from m in GL.GeneralLedgers.Where(m => m.choice == true)
                           select new
                               GL_Acct
                           {
                               GLNumber = m.GeneralLedger,
                               GLDescription = m.Description,
                               CategoryId = GL.Category
                           }).ToList();
                AccountingService srvc = new AccountingService();
                var srvcResponse = srvc.Adding_GLAccountBulk(gls);
                return Json(new { rcode = srvcResponse.respcode, msg = srvcResponse.msg }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { rcode = 2, msg = "Please select GLs to be added to " + GL.Category, JsonRequestBehavior.AllowGet });
            }
        }
        [Route("GL/Remove")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteGL(UpdateGenralLedgerModel GLCategory)
        {
            if (GLCategory.GeneralLedgers.Count(m => m.choice == true) > 0)
            {
                var item = (from m in GLCategory.GeneralLedgers.Where(n => n.Category == GLCategory.Category && n.choice == true)
                            select new deleteGl
                            {
                                GLNumber = m.GeneralLedger
                            }).ToList();
                AccountingService srv = new AccountingService();
                var response = srv.Deleting_GLaccount(item);
                return Json(new { rcode = response.respcode, msg = response.msg });
            }
            return Json(new { rcode = 0, msg = "Please select GLs to be remove" + GLCategory.Category });
        }

        [Route("GL/Remove")]
        [HttpGet]
        [OutputCache(Duration = 5)]
        public ActionResult DeleteGL()
        {
            AccountingService srvc = new AccountingService();

            UpdateGenralLedgerModel glView = new UpdateGenralLedgerModel();
            var gls = srvc.Get_GlbyCategory();
            var categories = srvc.get_GLdata().catItems;

            glView.GeneralLedgers = (from m in gls.data
                                     select new GeneralLedgerModel
                                     {
                                         Category = m.CategoryId.Trim(),
                                         GeneralLedger = m.GLcode.Trim(),
                                         Description = m.Description.Trim()
                                     }).ToList();

            glView.Categories = (from m in categories.Where(n => gls.data.Select(x => x.CategoryId).Contains(n.Category_id.ToString())).ToList()
                                 select new SelectListItem { Value = m.Category_id.ToString().Trim(), Text = m.Description.Trim() }).OrderBy(m => m.Text).ToList();
            return PartialView("_partialUpdateLedger", glView);
            //return Json(new { page = PartialToString.RenderPartialViewToString(this, "_partialUpdateLedger", glView), categorygl = GeneralLedgers }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("GL/CreateGL")]
        public ActionResult CreateGL()
        {
            GeneralLedgerViewModel GLDataEntry = new GeneralLedgerViewModel();
            AccountingService srv = new AccountingService();
            var response = srv.get_GLdata();
            GLDataEntry.Categories = (from m in response.catItems select new SelectListItem { Value = m.Category_id.ToString(), Text = m.Description }).OrderBy(m => m.Text).ToList();


            return PartialView("partialCreateGL", GLDataEntry);
        }
        [Route("GL/CreateGL")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGL(GeneralLedgerViewModel DataEntryGL)
        {
            if (DataEntryGL.Category != ""
                && !String.IsNullOrEmpty(DataEntryGL.GeneralLedger.GeneralLedger)
                && !String.IsNullOrEmpty(DataEntryGL.GeneralLedger.Description))
            {
                AccountingService srvc = new AccountingService();
                GL_Acct gl = new GL_Acct { CategoryId = DataEntryGL.Category, GLDescription = DataEntryGL.GeneralLedger.Description, GLNumber = DataEntryGL.GeneralLedger.GeneralLedger };
                var response = srvc.Adding_GLaccount(gl);
                return Json(new { rcode = response.respcode, msg = response.msg });
            }
            return Json(new { rcode = 0, msg = "Please fill-up all the field for Creating GL" });
        }

    }
}