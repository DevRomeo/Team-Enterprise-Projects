using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAT.Models;

namespace SAT.Controllers
{
    [Authorize]
    public class EDIReportController : Controller
    {
        // GET: EDIReport
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(EDIRequestReportModel request)
        {
            return View(request);
        }
        [HttpGet]
        public ActionResult requestReport()
        {

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult requestReport(EDIRequestReportModel request)
        {

            return View("Index", request);
        }
    }
}