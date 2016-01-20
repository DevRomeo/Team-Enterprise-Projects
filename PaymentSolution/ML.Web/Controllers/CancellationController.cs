using ML.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ML.PaymentSolution.Services.Contracts;
using ML.PaymentSolution.Services.Contracts.Models;

namespace ML.Web.Controllers
{
    public class CancellationController : Controller
    {
        private readonly IServiceFactory serviceFactory;

        public CancellationController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }
        //
        // GET: /Cancellation/
        [Route("Cancellation")]
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Route("Payment/Info")]
        [Authorize]
        [HttpGet]
        public ActionResult getPayment(string data)
        {
            var pay = serviceFactory.GetService<IPaymentSolution>();
            var d = pay.SearchEmployeeMethod(data,User.Identity.Name);
            CancellationList cancelList = new CancellationList();

            if (d.respcode == ResponseCode.Success)
            {
                cancelList.Cancellations = (from m in d.data
                                            select new CancellationModel
                                                {
                                                    Amount = m.amount,
                                                    FirstName = m.firstName,
                                                    Group = m.groupName,
                                                    Invoice = m.invNumber,
                                                    KPTN = m.KPTN,
                                                    LastName = m.lastName,
                                                    MiddleName = m.middleName,
                                                    CancellationCharge = m.cancelCharge

                                                }).OrderBy(m => m.FirstName).ToList();
                return Json(new { code = d.respcode, data = PartialToString.RenderPartialViewToString(this, "_partialCancellation", cancelList) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = d.msg, search = data }, JsonRequestBehavior.AllowGet);

            }
        }

        [Route("Payment/Cancel")]
        [Authorize]
        [HttpPost]
        public ActionResult cancelPayment(CancellationList data)
        {
            var cancelService = serviceFactory.GetService<IPaymentSolution>();
            List<SearchCancellationModel> cancel = (from m in data.Cancellations
                                                    where m.Option == true
                                                    select new SearchCancellationModel
                                                    {
                                                        amount = m.Amount,
                                                        firstName = m.FirstName,
                                                        middleName = m.MiddleName,
                                                        lastName = m.LastName,
                                                        groupName = m.Group,
                                                        KPTN = m.KPTN,
                                                        invNumber = m.Invoice,
                                                        cancelBy = User.Identity.Name,
                                                        cancelReason = m.Reason,
                                                        sendoutTable = m.KPTN,
                                                        cancelCharge = m.CancellationCharge
                                                    }).ToList();
            if (cancel.Count > 0)
            {
                var result = cancelService.CancellationEmployeeMethod(cancel, User.Identity.Name);
                System.Threading.Thread.Sleep(2000);
                return Json(new { Message = result.msg }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Message = "Please select a transaction to be cancelled" });
            }
        }
    }
}