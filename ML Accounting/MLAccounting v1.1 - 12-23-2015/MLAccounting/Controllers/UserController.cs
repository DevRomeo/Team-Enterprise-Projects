using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccountingWeb.Models;
using System.Web.Security;
using MLAccountingWeb.Security;
using Newtonsoft.Json;
using ML.AccountingSystemV1.Service;
using ML.AccountingSystemV1.Contract.Parameters;
using System.Globalization;
using ML.AccountingSystemV1.Contract;

namespace AccountingWeb.Controllers
{
    [HandleError(View = "UnexpectedError")]
    public class UserController : Controller
    {
        private readonly iAccountingService service;
        public UserController(iAccountingService service)
        {
            this.service = service;
        }
        //
        // GET: /User/
        [HttpGet]        
        [Route("")]
        [OutputCache(Duration = 10)]
        public ActionResult Login(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {                                          
                //ROSA94003233
                //if (!string.IsNullOrEmpty(returnUrl))
                //    return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]        
        [ValidateAntiForgeryToken]
        [Route("")]
        public ActionResult Login(LoginModel signIn, string returnUrl)
        {

            if (ModelState.IsValid)
            {

                //Retrieve Data from Database
                //AccountingService srvc = new AccountingService();
                var srvcLoginR = this.service.Login(new LoginParameters() { username = signIn.Username, password = signIn.Password });
                if (srvcLoginR.respcode == 1)
                {


                    string[] roles = srvcLoginR.DataGiven.job_title.Split('/');
                    CustomPrincipalSerializeModel userinfo = new CustomPrincipalSerializeModel
                    {
                        FirstName = srvcLoginR.DataGiven.first_name.Trim(),
                        LastName = srvcLoginR.DataGiven.sur_name.Trim(),
                        MiddleName = srvcLoginR.DataGiven.middle_name.Trim(),
                        UserId = Convert.ToInt32(signIn.Password),
                        roles = roles,
                        task = srvcLoginR.DataGiven.task
                    };
                    String fullname = new CultureInfo("en-US").TextInfo.ToTitleCase(userinfo.FirstName + " " + userinfo.MiddleName + " " + userinfo.LastName);
                    string userData = JsonConvert.SerializeObject(userinfo);
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, fullname, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData);
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (roles.Contains("User"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                if (FormsAuthentication.Authenticate(signIn.Username, signIn.Password))
                {
                    string[] roles = { "Admin" };
                    CustomPrincipalSerializeModel userinfo = new CustomPrincipalSerializeModel
                    {
                        FirstName = signIn.Username,
                        LastName = "",
                        MiddleName = "",
                        roles = roles
                    };

                    string userData = JsonConvert.SerializeObject(userinfo);
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, signIn.Username, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData);
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                    return RedirectToAction("Index", "Home");
                }
                ViewData.Add("servicemsg", srvcLoginR.message);
                //if (signIn.Username == "AccountingAdmin")
                //{
                //    string[] roles = { "Admin" };
                //    CustomPrincipalSerializeModel userinfo = new CustomPrincipalSerializeModel
                //    {
                //        FirstName = "Administrator",
                //        LastName = "",
                //        MiddleName = "", 
                //        roles = roles
                //    };
                //    string userData = JsonConvert.SerializeObject(userinfo);
                //    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, signIn.Username, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData);
                //    string encTicket = FormsAuthentication.Encrypt(ticket);
                //    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                //    Response.Cookies.Add(faCookie);
                //    return RedirectToAction("Index", "Home");
                //}
                return View("Login");
                //FormsAuthentication.SetAuthCookie(signIn.Username, false);
                //if (Url.IsLocalUrl(returnUrl))
                //{
                //    return Redirect(returnUrl);
                //}
                //else
                //{
                //    FormsAuthentication.RedirectFromLoginPage(signIn.Username, false);
                //    return RedirectToAction("Index", "Home");
                //}

            }
            else
            {
                return View("Login");
            }
        }

        private static string UppercaseWords(string value)
        {
            char[] array = value.ToCharArray();
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        [HttpPost]
        [Authorize]
        [Route("SingOut")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "User", null);
        }
    }
}