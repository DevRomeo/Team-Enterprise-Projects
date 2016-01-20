using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ML.PaymentSolution.Services.Contracts;
using PagedList;
using ML.Web.Models;
using ML.PaymentSolution.Services.Contracts.Models;

namespace ML.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IServiceFactory serviceFactory;
        public ReportsController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        [Route("paymentHistoryReport")]
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var groupdata = serviceFactory.GetService<IPaymentSolution>();
            var data = groupdata.GetAllGroups("wsclientuser", "wcfw0rdp@ass", User.Identity.Name);

            //Item list for Month
            IEnumerable<SelectListItem> monthlist = new[]
            {
                new SelectListItem{Value = "1",Text = "JAN"},
                new SelectListItem{Value = "2",Text = "FEB"},
                new SelectListItem{Value = "3",Text = "MAR"},
                new SelectListItem{Value = "4",Text = "APR"},
                new SelectListItem{Value = "5",Text = "MAY"},
                new SelectListItem{Value = "6",Text = "JUN"},
                new SelectListItem{Value = "7",Text = "JUL"},
                new SelectListItem{Value = "8",Text = "AUG"},
                new SelectListItem{Value = "9",Text = "SEP"},
                new SelectListItem{Value = "10",Text = "OCT"},
                new SelectListItem{Value = "11",Text = "NOV"},
                new SelectListItem{Value = "12",Text = "DEC"}
            };

            var daylist = Enumerable.Range(1, 31).Select(x => new SelectListItem
            {
                Value = x.ToString(CultureInfo.InvariantCulture),
                Text = x.ToString(CultureInfo.InvariantCulture)
            });

            var yearlist = Enumerable.Range(1900, (DateTime.Now.Year - 1900) + 1).Reverse().Select(x => new SelectListItem
            {
                Value = x.ToString(CultureInfo.InvariantCulture),
                Text = x.ToString(CultureInfo.InvariantCulture)
            });

            var model = new ReportModel
            {
                items = (from item in data.ResData.Where(s => s.NoOfEmployee != 0)
                         select new SelectListItem
                         {
                             Value = item.GroupName,
                             Text = item.GroupName
                         }).ToList(),
                month = new SelectList(monthlist, "Value", "Text"),
                day = new SelectList(daylist, "Value", "Text"),
                year = new SelectList(yearlist, "Value", "Text")
            };
            return PartialView("_partialReports", model);
        }

        [Authorize]
        [Route("ByGroupReports")]
        [HttpGet]
        public ActionResult byGroupList(string name, string historyname, DateTime dateselected)
        {
            string dateselect = dateselected.ToString("yyyy-MM-dd");
            var counter = 1;
            var groupdata = serviceFactory.GetService<IPaymentSolution>();
            var data = groupdata.RetrievePayments("wsclientuser", "wcfw0rdp@ass", User.Identity.Name);
            var items = (from item in data.ResData.Where(s => s.GroupName.Contains(name) && s.NumberOfEmployee != 0 && !string.IsNullOrEmpty(s.InvoiceNumber) && Convert.ToDateTime(s.DateCreated, CultureInfo.CurrentCulture).ToString("yyyy-MM-dd") == dateselect)
                         select new ML.Web.Models.ReportModel
                         {
                             employer = item.fullname,
                             groupname = item.GroupName,
                             amount = item.TotalAmount,
                             noofemployee = item.NumberOfEmployee,
                             invoicenumber = item.InvoiceNumber,
                             charge = item.TotalCharge,
                             status = item.Status,
                             processed = item.totalPayout.ToString(),
                             counter = counter++,
                             KPTN = item.KPTN
                         }).ToList();

            //ViewBag.selectedHistory = historyname;
            ////ViewBag.employer = items[0].employer;
            ////ViewBag.employer = User.Identity.Name;
            //ViewBag.groupname = name;
            //ViewBag.dateselect = dateselected.ToString("MMMM dd, yyyy");
            //if (items.Count == 0) { items = null; ViewBag.message = "No Record Found!"; }
            //return PartialView("_summaryreport", items);

            if (items.Count == 0)
            {
                items = null;
                return Json(new { code = 1002, Message = "No Record Found" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ViewBag.selectedHistory = historyname;
                ViewBag.employer = items[0].employer;
                ViewBag.groupname = name;
                ViewBag.dateselect = dateselected.ToString("MMMM dd, yyyy");
                return PartialView("_summaryreport", items);
            }
        }

        [Authorize]
        [Route("AllGroupReports")]
        [HttpGet]
        public ActionResult allGroupList(string historyname, DateTime dateselected)
        {

            string dateselect = dateselected.ToString("yyyy-MM-dd");
            var counter = 1;
            var groupdata = serviceFactory.GetService<IPaymentSolution>();
            var data = groupdata.RetrievePayments("wsclientuser", "wcfw0rdp@ass", User.Identity.Name);
            var c = (from item in data.ResData.Where(m => m.Status == "CLAIMED") select new ML.Web.Models.ReportModel { status = item.Status }).ToList();
            var items = (from item in data.ResData.Where(s => s.NumberOfEmployee != 0 && !string.IsNullOrEmpty(s.InvoiceNumber) && Convert.ToDateTime(s.DateCreated, CultureInfo.CurrentCulture).ToString("yyyy-MM-dd") == dateselect)
                         select new ML.Web.Models.ReportModel
                         {
                             employer = item.fullname,
                             groupname = item.GroupName,
                             amount = item.TotalAmount,
                             noofemployee = item.NumberOfEmployee,
                             invoicenumber = item.InvoiceNumber,
                             charge = item.TotalCharge,
                             status = item.Status,
                             counter = counter++,
                             processed = item.totalPayout.ToString(),
                             KPTN = item.KPTN
                         }).ToList();

            if (items.Count == 0)
            {
                items = null;
                return Json(new { code = 1002, Message = "No Record Found" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ViewBag.selectedHistory = historyname;
                ViewBag.employer = items[0].employer;
                ViewBag.groupname = "All";
                ViewBag.dateselect = dateselected.ToString("MMMM dd, yyyy");
                return PartialView("_summaryreport", items);
            }

        }

        [Authorize]
        [Route("AllCancelled")]
        [HttpGet]
        public ActionResult AllCancelledGroup(string historyname, DateTime date)
        {
            var groupdata = serviceFactory.GetService<IPaymentSolution>();
            var data = groupdata.CancellationReport(date.ToString("yyyy-MM-dd"), User.Identity.Name);

            if (data.respcode == ResponseCode.Success && data.data1.Count > 0)
            {
                var result = (from m in data.data1
                              select new Models.CancellationReportModel
                                  {
                                      CancelledBy = m.cancelledby,
                                      OtherCharge = m.otherCharge,
                                      Charge = m.charge,
                                      Principal = m.amount,
                                      CancelTime = m.cancelledTime.ToString("hh:mm tt"),
                                      Employee = string.Format("{0} {1} {2}", m.receiverFname, m.receiverMname, m.receiverLname),
                                      Group = m.groupName,
                                      InvoiceNo = m.invoiceno,
                                      KPTN = m.KPTN,
                                  }).ToList();

                ViewBag.selectedHistory = historyname;
                ViewBag.employer = data.Employeer;
                ViewBag.groupname = "All";
                ViewBag.dateselect = date.ToString("MMMM dd, yyyy");
                return PartialView("_partialCancellationReport", result);
            }
            else
            {
                //items = null;
                return Json(new { code = 1002, Message = "No Record Found" }, JsonRequestBehavior.AllowGet);
            }
            //return PartialView("_partialCancellationReport");
        }

        [Authorize]
        [Route("GroupCancelled")]
        [HttpGet]
        public ActionResult ByCancelledGroup(string name, string historyname, DateTime date)
        {
            var groupdata = serviceFactory.GetService<IPaymentSolution>();
            var data = groupdata.CancellationReport(date.ToString("yyyy-MM-dd"), User.Identity.Name);

            if (data.respcode == ResponseCode.Success)
            {
                var result = (from m in data.data1
                              where m.groupName == name
                              select new Models.CancellationReportModel
                              {
                                  CancelledBy = m.cancelledby,
                                  OtherCharge = m.otherCharge,
                                  Charge = m.charge,
                                  Principal = m.amount,
                                  CancelTime = m.cancelledTime.ToString("hh:mm tt"),
                                  Employee = string.Format("{0} {1} {2}", m.receiverFname, m.receiverMname, m.receiverLname),
                                  Group = m.groupName,
                                  InvoiceNo = m.invoiceno,
                                  KPTN = m.KPTN
                              }).ToList();

                if (result.Count > 0)
                {
                    ViewBag.selectedHistory = historyname;
                    ViewBag.employer = data.Employeer;
                    ViewBag.groupname = name;
                    ViewBag.dateselect = date.ToString("MMMM dd, yyyy");
                    return PartialView("_partialCancellationReport", result);
                }
                else
                {
                    return Json(new { code = 1002, Message = "No Record Found" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //items = null;
                return Json(new { code = 1002, Message = "No Record Found" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}