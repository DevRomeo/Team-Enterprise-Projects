using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ML.PaymentSolution.Services.Contracts;
using ML.PaymentSolution.Services.Contracts.Models;
using ML.Web.Models;
using PagedList;

namespace ML.Web.Controllers
{

    public class HomeController : Controller
    {

        private readonly IServiceFactory serviceFactory;

        public HomeController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        [Route("")]
        [Authorize]
        [OutputCache(Duration = 5)]

        public ActionResult Index(string searchString, int? page)
        {
            var pay = serviceFactory.GetService<IPaymentSolution>();
            var data = pay.RetrievePayments("wsclientuser", "wcfw0rdp@ass", User.Identity.Name);
            if (!String.IsNullOrEmpty(searchString))
            {
                data.ResData = data.ResData.Where(s => s.InvoiceNumber.Contains(searchString));
            }
            var home = (from m in data.ResData
                        select new SearchPaymentModel
                        {
                            InvoiceNumber = m.InvoiceNumber,
                            TotalAmount = m.TotalAmount,
                            NumberOfEmployee = m.NumberOfEmployee,
                            Status = m.Status,
                            GroupName = m.GroupName,
                            totalPayout = m.totalPayout,
                            PayId = m.PayId
                        }).ToList();

            data.ResData = home;
            const int pageSize = 8;
            var pageNumber = (page ?? 1);
            return View(data.ResData.ToPagedList(pageNumber, pageSize));
        }

        [Route("reloadindex")]
        [HttpGet]
        [Authorize]
        public PartialViewResult refreshindex(int? page)
        {

            var pay = serviceFactory.GetService<IPaymentSolution>();
            var data = pay.RetrievePayments("wsclientuser", "wcfw0rdp@ass", User.Identity.Name);

            const int pageSize = 8;
            var pageNumber = (page ?? 1);
            return PartialView("_partialIndex", data.ResData.ToPagedList(pageNumber, pageSize));
        }

    }
}