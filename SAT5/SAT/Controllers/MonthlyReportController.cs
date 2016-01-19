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
    public class MonthlyReportController : Controller
    {
        // GET: MonthlyReport
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(MonthlyReportRequestModel request)
        {
            return View(request);
        }
        [HttpPost]
        public ActionResult requestReport(MonthlyReportRequestModel request)
            {

            return View("Index", request);
        }
        [HttpGet]
        public ActionResult requestReport()
        {

            return RedirectToAction("Index");
        }

        // GET: MonthlyReport/Details/5
       
    }
}
