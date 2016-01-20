using System.Linq;
using System.Web.Mvc;
using ML.PaymentSolution.Services.Contracts;
using ML.PaymentSolution.Services.Contracts.Models;
using ML.Web.Models;
using PagedList;
using System.Web;

namespace ML.Web.Controllers
{
    public class GroupController : Controller
    {
        private readonly IServiceFactory serviceFactory;

        public GroupController(IServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        [Route("Group")]
        [Authorize]
        [OutputCache(Duration = 5)]
        public ActionResult GroupList(int? page)
        {
            var grpdata = serviceFactory.GetService<IPaymentSolution>();
            var data = grpdata.GetAllGroups("wsclientuser", "wcfw0rdp@ass", User.Identity.Name);

            const int pageSize = 8;
            var pageNumber = (page ?? 1);

            return View(data.ResData.OrderBy(m => m.GroupName).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [Route("Group/Add")]
        [Authorize]
        public ActionResult Create(string groupname)
        {
            var grpdata = serviceFactory.GetService<IPaymentSolution>();

            var res = grpdata.AddGroup("wsclientuser", "wcfw0rdp@ass", groupname, User.Identity.Name);

            switch (res.ResCode)
            {
                case ResponseCode.Exception:
                    return Json(new { code = res.ResCode, Message = res.ResMessage });
                case ResponseCode.Success:
                    return Json(new { code = res.ResCode, action = Url.Action("grouplist", "Group"), Message = res.ResMessage });
                default:
                    return Json(new { code = res.ResCode, Message = res.ResMessage });
            }
        }

        [HttpGet]
        [Route("Group/Edit")]
        [Authorize]
        public ActionResult Edit(string groupid)
        {
            var editgroup = serviceFactory.GetService<IPaymentSolution>();
            var data = editgroup.SearchGroup("wsclientuser", "wcfw0rdp@ass", groupid, User.Identity.Name);
            data.EmployeeData.ResData = data.EmployeeData.ResData.OrderBy(m => m.LastName);
            return View("Edit", data);
        }

        [HttpPost]
        [Route("Group/Update")]
        [Authorize]
        public ActionResult Update(string groupid, string newgroupname)
        {
            var updategroup = serviceFactory.GetService<IPaymentSolution>();

            var resp = updategroup.EditGroup("wsclientuser", "wcfw0rdp@ass", groupid, newgroupname.Trim(), User.Identity.Name);

            if (resp.ResCode == ResponseCode.Success)
            {
                return Json(new { code = resp.ResCode, Message = resp.ResMessage, GroupName = newgroupname });
            }
            else
            {
                return Json(new { Message = resp.ResMessage });
            }
        }

        [HttpGet]
        [Route("groupdelete")]
        [Authorize]
        public ActionResult Delete(string groupid)
        {
            var delete = serviceFactory.GetService<IPaymentSolution>();
            var data = delete.SearchGroup("wsclientuser", "wcfw0rdp@ass", groupid, User.Identity.Name);

            return View("Delete", data);
        }

        [Route("Group/Delete")]
        [HttpPost]
        [Authorize]
        public ActionResult DeleteGroup(string groupid)
        {
            var delete = serviceFactory.GetService<IPaymentSolution>();
            var resp = delete.DeleteGroup("wsclientuser", "wcfw0rdp@ass", groupid, User.Identity.Name);

            switch (resp.ResCode)
            {
                case ResponseCode.Success:
                    return Json(new { code = ResponseCode.Success, action = Url.Action("GroupList", "Group") });
                default:
                    return Json(new { errCode = resp.ResCode, Message = resp.ResMessage });
            }
        }

        [Route("Group/AddEmployee")]
        [HttpPost]
        [Authorize]
        public ActionResult addmember(string groupid, string empid, double salary)
        {

            var _service = serviceFactory.GetService<IPaymentSolution>();
            var member = new MemberModel
            {
                GroupId = groupid,
                EmpId = empid,
                Salary = salary
            };

            var data = _service.AddMember("wsclientuser", "wcfw0rdp@ass", member);

            //if (data.ResCode == ResponseCode.ErrorAddingGroup)
            //{
            //    return Json(new { rescode = "1000", msg = "Duplicate member." });
            //}
            if (data.ResCode == ResponseCode.Success)
            {
                var reload = _service.SearchGroup("wsclientuser", "wcfw0rdp@ass", groupid, User.Identity.Name);
                reload.EmployeeData.ResData = reload.EmployeeData.ResData.OrderBy(m => m.LastName);
                return Json(new { code = data.ResCode, data = PartialToString.RenderPartialViewToString(this, "_partialeditemployeelist", reload.EmployeeData) });
            }
            else
            {
                return Json(new { Message = data.ResMessage });
            }
        }

        [Route("Group/Employee")]
        [HttpPost]
        [Authorize]
        public ActionResult getemployees(string groups)
        {
            var emplist = serviceFactory.GetService<IPaymentSolution>();
            var data = emplist.FilterEmployee("wsclientuser", "wcfw0rdp@ass", User.Identity.Name, groups);
            var employee = new EmployeeViewModel();
            var items = (from item in data.ResData.OrderBy(m => m.LastName)
                         where item.GroupId != groups
                         select new SelectListItem
                         {
                             Value = item.EmpId,
                             Text = item.LastName + ", " + item.FirstName + " " + item.MiddleName
                         }).ToList();

            employee.employee = items;
            return Json(new { code = data.ResCode, data = PartialToString.RenderPartialViewToString(this, "_Index", employee) });
            //return PartialView("_Index", employee);


        }

        [Route("Group/EditEmployee")]
        [HttpGet]
        [Authorize]
        public ActionResult getemployeebyid(string empid)
        {
            var emplist = serviceFactory.GetService<IPaymentSolution>();
            var data = emplist.GetAllEmployee("wsclientuser", "wcfw0rdp@ass", User.Identity.Name);
            var employee = new EmployeeViewModel();
            var items = (from item in data.ResData.Where(x => x.EmpId == empid)
                         select new EmployeeViewModel
                         {
                             kycId = item.EmpId,
                             employeelist = item.LastName + ", " + item.FirstName + " " + item.MiddleName
                         }).FirstOrDefault();

            //employee.employee = items;

            return PartialView("_PartialUpdateSalary", items);
        }

        [Route("Group/RemoveEmployee")]
        [HttpPost]
        [Authorize]
        public ActionResult removemember(string groupid, string empid)
        {
            var member = serviceFactory.GetService<IPaymentSolution>();
            var model = new MemberModel
            {
                GroupId = groupid,
                EmpId = empid
            };
            member.RemoveMember("wsclientuser", "wcfw0rdp@ass", model);

            var reloadgroup = member.SearchGroup("wsclientuser", "wcfw0rdp@ass", groupid, User.Identity.Name);
            reloadgroup.EmployeeData.ResData = reloadgroup.EmployeeData.ResData.OrderBy(m => m.LastName);
            return PartialView("_partialeditemployeelist", reloadgroup.EmployeeData);
        }

        [Route("Group/UpdateEmployee")]
        [HttpPost]
        [Authorize]
        public ActionResult editmembersalary(string groupid, string empid, double salary)
        {
            if (salary <= 50000.00)
            {
                var member = serviceFactory.GetService<IPaymentSolution>();
                var model = new MemberModel
                {
                    GroupId = groupid,
                    EmpId = empid,
                    Salary = salary
                };

                member.RemoveMember("wsclientuser", "wcfw0rdp@ass", model);

                var data = member.AddMember("wsclientuser", "wcfw0rdp@ass", model);
                if (data.ResCode == ResponseCode.Success)
                {
                    var reload = member.SearchGroup("wsclientuser", "wcfw0rdp@ass", groupid, User.Identity.Name);
                    reload.EmployeeData.ResData = reload.EmployeeData.ResData.OrderBy(m => m.LastName);
                    return Json(new { code = data.ResCode, data = PartialToString.RenderPartialViewToString(this, "_partialeditemployeelist", reload.EmployeeData) });

                }
                else
                {
                    return Json(new { Message = data.ResMessage });
                }
            }
            else
            {
                return Json(new { Message = "Salary must be below 50k" });
            }
        }
    }
}