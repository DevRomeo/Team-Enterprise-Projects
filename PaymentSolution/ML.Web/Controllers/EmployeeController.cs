using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ML.PaymentSolution.Services.Contracts;
using ML.PaymentSolution.Services.Contracts.Models;
using ML.Web.Models;
using PagedList;

namespace ML.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IServiceFactory serviceFactory;

        public EmployeeController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        [Authorize]
        [Route("Employee")]
        [OutputCache(Duration = 5)]
        public ActionResult EmployeeList(int? page)
        {
            var emplist = serviceFactory.GetService<IPaymentSolution>()
                        .GetAllEmployee("wsclientuser", "wcfw0rdp@ass", User.Identity.Name);
            //var list = emplist.GetAllEmployee("wsclientuser", "wcfw0rdp@ass", User.Identity.Name);            
            const int pageSize = 8;
            var pageNumber = (page ?? 1);            
            return View(emplist.ResData.OrderBy(m => m.LastName).ToPagedList(pageNumber, pageSize));
        }

        [Route("Employee/Edit")]
        [Authorize]
        [OutputCache(Duration = 20)]
        public ActionResult Edit(string empid)
        {
            var edit = serviceFactory.GetService<IPaymentSolution>();
            var data = edit.SearchEmployee("wsclientuser", "wcfw0rdp@ass", empid, User.Identity.Name);

            if (data.ResCode == ResponseCode.Success)
            {



                //Item list for Gender
                IEnumerable<SelectListItem> items = new[]
            {
                new SelectListItem{Value = "Male", Text = "Male"},
                new SelectListItem{Value = "Female",Text = "Female"}
            };

                //Item list for Country
                IEnumerable<SelectListItem> countrylist = new[]
            {
                new SelectListItem{Text = "Philippines",Value = "Philippines"}, 
                new SelectListItem{Text = "United States",Value = "United States"}
            };

                //Item list for Month
                IEnumerable<SelectListItem> monthlist = new[]
            {
                new SelectListItem{Value = "1",Text = "JANUARY"},
                new SelectListItem{Value = "2",Text = "FEBRUARY"},
                new SelectListItem{Value = "3",Text = "MARCH"},
                new SelectListItem{Value = "4",Text = "APRIL"},
                new SelectListItem{Value = "5",Text = "MAY"},
                new SelectListItem{Value = "6",Text = "JUNE"},
                new SelectListItem{Value = "7",Text = "JULY"},
                new SelectListItem{Value = "8",Text = "AUGUST"},
                new SelectListItem{Value = "9",Text = "SEPTEMBER"},
                new SelectListItem{Value = "10",Text = "OCTOBER"},
                new SelectListItem{Value = "11",Text = "NOVEMBER"},
                new SelectListItem{Value = "12",Text = "DECEMBER"}
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

                data.Gender = new SelectList
                    (
                        items, "Value", "Text"
                    );

                data.Country = new SelectList
                    (
                        countrylist, "Value", "Text"
                    );

                data.Month = new SelectList
                    (
                        monthlist, "Value", "Text"
                    );

                var model = new UpdateEmployeeViewModel
                {
                    idno = data.EmployeeModelInfo.CustID,
                    lastname = data.EmployeeModelInfo.LastName,
                    firstname = data.EmployeeModelInfo.FirstName,
                    middlename = data.EmployeeModelInfo.MiddleName,
                    address = data.EmployeeModelInfo.PermanentAddress,
                    birthday = data.EmployeeModelInfo.BirthDate,
                    mobileno = data.EmployeeModelInfo.Mobile,
                    phoneno = data.EmployeeModelInfo.PhoneNo,
                    email = data.EmployeeModelInfo.Email,
                    selectedcountry = data.EmployeeModelInfo.Country,
                    selectedgender = data.EmployeeModelInfo.Gender,
                    nationality = data.EmployeeModelInfo.Nationality,
                    selectedgovidtype = data.EmployeeModelInfo.GovtID,
                    govid2 = data.EmployeeModelInfo.GovIDNo,
                    selectedmonth = data.EmployeeModelInfo.BirthDate.Month.ToString(),
                    selectedday = data.EmployeeModelInfo.BirthDate.Day,
                    selectedyear = data.EmployeeModelInfo.BirthDate.Year,

                    month = new SelectList
                    (
                        monthlist, "Value", "Text"
                    ),

                    day = new SelectList
                    (
                        daylist, "Value", "Text"
                    ),

                    year = new SelectList
                    (
                        yearlist, "Value", "Text"
                    ),

                    _gender = new SelectList
                        (
                            items, "Value", "Text"
                        )

                };

                return View("Edit", model);
            }
            else
            {
                return Json(new { Message = data.ResMessage });
            }
        }

        [Route("Employee/Update")]
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEmployee(UpdateEmployeeViewModel model)
        {

            var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));

            model.errors = allErrors;

            if (!ModelState.IsValid)
            {
                return Json(new { code = PayrollCode.ModelError, Message = model.errors });
            }

            model.month = new[]
            {
                new SelectListItem {Value = "1", Text = "JANUARY"},
                new SelectListItem {Value = "2", Text = "FEBRUARY"},
                new SelectListItem {Value = "3", Text = "MARCH"},
                new SelectListItem {Value = "4", Text = "APRIL"},
                new SelectListItem {Value = "5", Text = "MAY"},
                new SelectListItem {Value = "6", Text = "JUNE"},
                new SelectListItem {Value = "7", Text = "JULY"},
                new SelectListItem {Value = "8", Text = "AUGUST"},
                new SelectListItem {Value = "9", Text = "SEPTEMBER"},
                new SelectListItem {Value = "10", Text = "OCTOBER"},
                new SelectListItem {Value = "11", Text = "NOVEMBER"},
                new SelectListItem {Value = "12", Text = "DECEMBER"}
            };
            model.day = Enumerable.Range(1, 31).Select(x => new SelectListItem
            {
                Value = x.ToString(CultureInfo.InvariantCulture),
                Text = x.ToString(CultureInfo.InvariantCulture)
            });

            model.year = Enumerable.Range(1900, (DateTime.Now.Year - 1900) + 1).Reverse().Select(x => new SelectListItem
            {
                Value = x.ToString(CultureInfo.InvariantCulture),
                Text = x.ToString(CultureInfo.InvariantCulture)
            });

            model._gender = new[]
            {
                new SelectListItem {Value = "Male",Text = "Male"}, 
                new SelectListItem {Value = "Female",Text = "Female"}
            };

            var update = serviceFactory.GetService<IPaymentSolution>();
            var kycmodel = new KycModel
            {
                CustID = model.idno,
                LastName = model.lastname,
                FirstName = model.firstname,
                MiddleName = model.middlename,
                PermanentAddress = model.address,
                BirthDate = Convert.ToDateTime(model.selectedyear + "-" + model.selectedmonth + "-" + model.selectedday),
                Mobile = model.mobileno,
                PhoneNo = model.phoneno,
                Email = model.email,
                Country = model.selectedcountry,
                Gender = model.selectedgender,
                Nationality = model.nationality,
                GovtID = model.selectedgovidtype,
                GovIDNo = model.govid2
            };


            var data = update.UpdateEmployee("wsclientuser", "wcfw0rdp@ass", "boswebserviceusr", "boyursa805", kycmodel);
            if (data.ResCode == ResponseCode.Success)
            {
                return Json(new { code = data.ResCode, Message = data.ResMessage, action = Url.Action("EmployeeList", "Employee") });
            }
            else
            {
                return Json(new { Message = data.ResMessage });
            }
            //switch (data.ResCode)
            //{
            //    case ResponseCode.Success:

            //    case ResponseCode.Exception:
            //        return Json(new { code = data.ResCode, Message = data.ResMessage });
            //    default:
            //        return Json(new { Message = "Something went wrong while updating this employee Please Try again later" });

            //}
        }


        [Route("Employee/Create")]
        [Authorize]
        [OutputCache(Duration = 60)]
        public ActionResult Create()
        {
            var model = new EmployeeViewModel
            {
                month = new[]
                {
                  new SelectListItem{Value = "1",Text = "JANUARY"},
                  new SelectListItem{Value = "2",Text = "FEBRUARY"},
                  new SelectListItem{Value = "3",Text = "MARCH"},
                  new SelectListItem{Value = "4",Text = "APRIL"},
                  new SelectListItem{Value = "5",Text = "MAY"},
                  new SelectListItem{Value = "6",Text = "JUNE"},
                  new SelectListItem{Value = "7",Text = "JULY"},
                  new SelectListItem{Value = "8",Text = "AUGUST"},
                  new SelectListItem{Value = "9",Text = "SEPTEMBER"},
                  new SelectListItem{Value = "10",Text = "OCTOBER"},
                  new SelectListItem{Value = "11",Text = "NOVEMBER"},
                  new SelectListItem{Value = "12",Text = "DECEMBER"}
                },

                day = Enumerable.Range(1, 31).Select(x => new SelectListItem
                {
                    Value = x.ToString(CultureInfo.InvariantCulture),
                    Text = x.ToString(CultureInfo.InvariantCulture)
                }),

                year = Enumerable.Range(1900, (DateTime.Now.Year - 1900) + 1).Reverse().Select(x => new SelectListItem
                {
                    Value = x.ToString(CultureInfo.InvariantCulture),
                    Text = x.ToString(CultureInfo.InvariantCulture)
                })
            };

            return View(model);
        }

        [Route("Employee/Save")]
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Save(EmployeeViewModel model, int? page)
        {
            var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
            model.errors = allErrors;

            if (!ModelState.IsValid)
            {
                return Json(new { code = PayrollCode.ModelError, Message = model.errors });
            }

            KycModel kyc = new KycModel
            {
                FirstName = model.firstname,
                LastName = model.lastname,
                MiddleName = model.middlename,
                Country = model.selectedcountry,
                Gender = model.selectedgender,
                BirthDate = Convert.ToDateTime(string.Format("{0}-{1}-{2}", model.selectedyear, model.selectedmonth, model.selectedday)).Date,
                PhoneNo = model.phoneno,
                Mobile = model.mobileno,
                Email = model.email,
                PermanentAddress = model.address,
                Nationality = model.nationality,
                GovtID = model.selectedgovidtype,
                GovIDNo = model.govid2
            };


            var data = serviceFactory.GetService<IPaymentSolution>().AddEmployee("wsclientuser", "wcfw0rdp@ass", User.Identity.Name, kyc);
            if (data.ResCode == ResponseCode.Success)
            {
                return Json(new { code = data.ResCode, Message = data.ResMessage, action = Url.Action("EmployeeList", "Employee") });
            }
            else
            {
                return Json(new { Message = data.ResMessage }, JsonRequestBehavior.AllowGet);
            }
            //switch (data.ResCode)
            //{
            //    case ResponseCode.Success:

            //    case ResponseCode.Exception:
            //        return Json(new { code = data.ResCode, Message = data.ResMessage }, JsonRequestBehavior.AllowGet);
            //    default:
            //        return Json(new { Message = "Something went wrong while registering an employee" }, JsonRequestBehavior.AllowGet);
            //}
        }


    }
}