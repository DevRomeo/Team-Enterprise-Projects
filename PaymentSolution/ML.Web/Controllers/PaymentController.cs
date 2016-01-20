using System;
using System.Linq;
using System.Web.Mvc;
using ML.PaymentSolution.Services.Contracts;
using PagedList;
using PaymentModel = ML.Web.Models.PaymentModel;
using System.Collections.Generic;
using System.IO;

namespace ML.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IServiceFactory serviceFactory;

        public PaymentController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        [Route("Payments")]
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var groupdata = serviceFactory.GetService<IPaymentSolution>();
            var data = groupdata.GetAllGroups("wsclientuser", "wcfw0rdp@ass", User.Identity.Name);

            var payment = new PaymentModel();
            var items = (from item in data.ResData
                         .Where(m => m.NoOfEmployee > 0)
                         select new SelectListItem
                         {
                             Value = item.GroupId,
                             Text = item.GroupName                              
                         }).ToList();

            //payment.Balance = data.WalletBalance;
            payment.group = items;


            return PartialView("_Create", payment);
        }

        [Route("Payment/Group")]
        [HttpGet]
        [Authorize]
        public PartialViewResult SelectGroup(string id)
        {
            var selected = serviceFactory.GetService<IPaymentSolution>();
            var data = selected.SearchGroup("wsclientuser", "wcfw0rdp@ass", id, User.Identity.Name);
            var payment = new PaymentModel
            {
                noofemployee = data.NoOfEmployee,
                remitance = data.TotalSalary,
                charge = data.TotalCharges,
                total = data.TotalSalary + data.TotalCharges
            };

            return PartialView("_refreshpayment", payment);

        }

        [Route("Payment/Create")]
        [HttpPost]
        [Authorize]
        public ActionResult Create(string id)
        {
            
            var create = serviceFactory.GetService<IPaymentSolution>();
            var searchgroup = create.SearchGroup("wsclientuser", "wcfw0rdp@ass", id, User.Identity.Name);
            if (searchgroup.NoOfEmployee != 0)
            {
                var pay = (from item in searchgroup.EmployeeData.ResData
                           select new PaymentSolution.Services.Contracts.Models.PaymentModel
                           {
                               GroupId = id,
                               LastName = item.LastName,
                               Charge = item.Charge,
                               FirstName = item.FirstName,
                               MiddleName = item.MiddleName,
                               EmpId = item.EmpId,
                               Amount = item.Salary,
                               KycId = item.KycId,
                               Status = "Open",
                               CreatedBy = User.Identity.Name,
                               groupName = searchgroup.GroupName
                           });


                List<PaymentSolution.Services.Contracts.Models.PaymentModel> lp = pay.ToList();
                if (lp.Where(m => m.Charge == 0).Count() >= 1)
                {
                    return Json(new { Message = "Unable to process transaction. Transaction amount exceeds to its limit. Please contact ML Helpdesk (09479992754 or 09479992755) for inquiries." });
                }


                var data = create.CreatePayment("wsclientuser", "wcfw0rdp@ass", lp);
                if (data.ResCode == PaymentSolution.Services.Contracts.Models.ResponseCode.Success)
                {
                    return Json(new { code = data.ResCode, data = PartialToString.RenderPartialViewToString(this, "_viewer", data) });
                }
                else
                {
                    return Json(new { Message = data.ResMessage });
                }
            }
            else
            {
                return Json(new { Message = searchgroup.ResMessage });
            }
        }

        //public string RenderPartialViewToString(Controller thisController, string viewName, object model)
        //{
        //    // assign the model of the controller from which this method was called to the instance of the passed controller (a new instance, by the way)
        //    thisController.ViewData.Model = model;

        //    // initialize a string builder
        //    using (StringWriter sw = new StringWriter())
        //    {
        //        // find and load the view or partial view, pass it through the controller factory
        //        ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(thisController.ControllerContext, viewName);
        //        ViewContext viewContext = new ViewContext(thisController.ControllerContext, viewResult.View, thisController.ViewData, thisController.TempData, sw);

        //        // render it
        //        viewResult.View.Render(viewContext, sw);

        //        //return the razorized view/partial-view as a string
        //        return sw.ToString();
        //    }
        //}

        //private string RenderViewToString(string ViewName, object Model)
        //{
        //    ViewData.Model = Model;
        //    using (var sw = new StringWriter())
        //    {

        //        var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, ViewName);
        //        var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
        //        viewResult.View.Render(viewContext, sw);
        //        viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
        //        return sw.GetStringBuilder().ToString();

        //    }
        //}

        [Route("Payment/Reprint")]
        [HttpGet]
        [Authorize]
        public PartialViewResult ReprintPayment(string invoiceno)
        {
            if (invoiceno.Trim() != "")
            {
                var reprint = serviceFactory.GetService<IPaymentSolution>();
                var data = reprint.Reprint("wsclientuser", "wcfw0rdp@ass", invoiceno);
                data.InvoiceNumber = invoiceno;
                data.PaymentModel = data.PaymentModel.OrderBy(m => m.LastName);
                return PartialView("_reprintviewer", data);


            }
            else
            {
                return PartialView("_reprintviewer", new ML.PaymentSolution.Services.Contracts.Responses.CreatePaymentResponse());
            }
        }

        [Route("Payment/Delete")]
        [HttpGet]
        [Authorize]
        public PartialViewResult RemovePayment(string payid, int? page)
        {
            var pay = serviceFactory.GetService<IPaymentSolution>();
            var data = pay.DeletePayment("wsclientuser", "wcfw0rdp@ass", payid, User.Identity.Name);

            var reloadpayments = pay.RetrievePayments("wsclientuser", "wcfw0rdp@ass", User.Identity.Name);

            const int pageSize = 8;
            var pageNumber = (page ?? 1);

            return PartialView("_partialIndex", reloadpayments.ResData.ToPagedList(pageNumber, pageSize));
        }

    }
}