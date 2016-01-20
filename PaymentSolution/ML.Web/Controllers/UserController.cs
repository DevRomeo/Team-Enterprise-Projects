using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ML.Web.Models;
using ML.PaymentSolution.Services.Contracts;
using ML.PaymentSolution.Services.Contracts.Responses;
using Newtonsoft.Json;
using ML.PaymentSolution.Services.Contracts.Models;
using System.Text;
using System.Web.Security;

namespace ML.Web.Controllers
{
    enum PayrollCode
    {
        Success = 1001,
        LoginInputRequired = 1004,
        ModelError = 2000

    }
    public class UserController : Controller
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IAuthorizationProvider _authorizationProvider;

        public UserController(IServiceFactory serviceFactory, IAuthorizationProvider authorizationProvider)
        {
            _serviceFactory = serviceFactory;
            _authorizationProvider = authorizationProvider;
        }

        [HttpGet]
        [Route("login")]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel
            {
                RedirectUrl = returnUrl
            };

            if (TempData["model"] != null)
                model = TempData["model"] as LoginViewModel;
            return View("UserLogin", model);
        }

        public ActionResult RedirectToDefault()
        {
            string[] roles = Roles.GetRolesForUser();
            if (roles.Contains("Admin"))
            {
                return RedirectToAction("GroupList", "Group");
            }
            else if (roles.Contains("User"))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("EmployeeList", "Employee");
            }
        }

        [HttpPost]
        [Route("Auth")]
        public ActionResult Authenticate(string username, string password, string redirecturl)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username))
            {
                return Json(new { code = PayrollCode.LoginInputRequired, Message = "The username field is required." });
            }
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            {
                return Json(new { code = PayrollCode.LoginInputRequired, Message = "The password field is required." });
            }

            var loginService = _serviceFactory.GetService<IPaymentSolution>();

            var validLogin = loginService.Login("wsclientuser", "wcfw0rdp@ass", username, password);
            switch (validLogin.ResCode)
            {
                case ResponseCode.Success:
                    _authorizationProvider.SetAuthCookie(username, true);

                    if (redirecturl != null && redirecturl != "/" && redirecturl != "")
                        return Json(new { code = validLogin.ResCode, action = redirecturl });
                    else
                        return Json(new { code = validLogin.ResCode, action = Url.Action("Index", "Home") });
                case ResponseCode.Userdidnotmatch:
                    return Json(new { code = validLogin.ResCode, Message = validLogin.ResMessage });
                case ResponseCode.Fatal:
                    return Json(new { code = validLogin.ResCode, Message = validLogin.ResMessage });
                case ResponseCode.Exception:
                    return Json(new { code = validLogin.ResCode, Message = validLogin.ResMessage });
                case ResponseCode.InvalidAccount:
                    return Json(new { code = validLogin.ResCode, Message = validLogin.ResMessage });
                case ResponseCode.StatusAdmin:
                    return Json(new { code = validLogin.ResCode, Message = validLogin.ResMessage });
                default:
                    return Json(new { Message = "Something went wrong on Login Please try again later" });
            }
        }

        [Route("logout")]
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            _authorizationProvider.SignOut();

            return Json(new { action = Url.Action("Login", "User") });

        }

        [Route("User/Save")]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult RegisterUser(RegisterAccountModel model)
        {
            var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));

            model.errors = allErrors;

            if (!ModelState.IsValid)
            {
                return Json(new { code = PayrollCode.ModelError, Message = model.errors });
            }


            var reg = _serviceFactory.GetService<IPaymentSolution>();
            var result = reg.RegisterUser("wsclientuser", "wcfw0rdp@ass", "boswebserviceusr", "boyursa805", "", model.firstname, model.lastname, model.middlename, "", ""
                , model.selectedcountry, model.selectedgender, model.selectedyear + "-" + model.selectedmonth + "-" + model.selectedday, "", "", "", "", model.phoneno,
                model.mobileno
                , model.email, User.Identity.Name, 0, "", "", "", model.address, model.nationality, "", "", model.selectedgovidtype, model.govid2, model.username, model.password, model.Wallet, model.Kyc, model.DeviceID);

            model.month = new[]
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

            if (result.ResCode == ResponseCode.Success)
            {
                return Json(new { code = result.ResCode, Message = result.ResMessage, action = Url.Action("Login", "User") });
            }
            else
            {
                return Json(new { code = result.ResCode, Message = result.ResMessage });
            }

            //switch (result.ResCode)
            //{
            //    case ResponseCode.Success:
            //        return Json(new { code = result.ResCode, Message = result.ResMessage, action = Url.Action("Login", "User") });
            //    case ResponseCode.DeviceAlreadyExist:
            //        return Json(new { code = result.ResCode, Message = result.ResMessage });
            //    case ResponseCode.InvalidWalletAccount:
            //        return Json(new { code = result.ResCode, Message = result.ResMessage });
            //    case ResponseCode.AccountAlreadyExist:
            //        return Json(new { code = result.ResCode, Message = result.ResMessage });
            //    case ResponseCode.InvalidCredentials:
            //        return Json(new { code = result.ResCode, Message = result.ResMessage });
            //    case ResponseCode.Exception:
            //        return Json(new { code = result.ResCode, Message = result.ResMessage });
            //    case ResponseCode.UserNameExist:
            //        return Json(new { code = result.ResCode, Message = result.ResMessage });
            //    case ResponseCode.WalletnoExist:
            //        return Json(new { code = result.ResCode, Message = result.ResMessage });
            //    case ResponseCode.EmailAddExist:
            //        return Json(new { code = result.ResCode, Message = result.ResMessage });
            //}

            //return View("Registration", model);
        }

        [Route("Registration")]
        public ActionResult Register()
        {
            var model = new RegisterAccountModel();
            if (TempData["wallet"] != null)
            {
                model = (RegisterAccountModel)JsonConvert.DeserializeObject(Convert.ToString(TempData["wallet"]), typeof(RegisterAccountModel));
            }
            else
            {
                model = new RegisterAccountModel
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
                    }),
                    isValidMobile = false
                };
            }
            return View("Registration", model);
        }

        [Route("ChangePassword")]
        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(string _currpass, string _password, string confirm)
        {
            if (_password.ToLower() != confirm.ToLower())
                return Json(new { rescode = 1004, msg = "Confirm password did not match." });

            var pass = _serviceFactory.GetService<IPaymentSolution>();
            var data = pass.UpdtePassword("wsclientuser", "wcfw0rdp@ass", User.Identity.Name, _currpass, _password);
            switch (data.ResCode)
            {
                case ResponseCode.InvalidAccount:
                    return Json(new { code = ResponseCode.InvalidAccount, Message = data.ResMessage });
                case ResponseCode.UpdatePassSuccess:
                    return Json(new { code = ResponseCode.UpdatePassSuccess, Message = data.ResMessage });
                default:
                    return Json(new { Message = data.ResMessage });
            }
        }

        [Route("Auth/MLWallet")]
        [HttpPost]
        public ActionResult GetWalletInfo(string wallet, string walletP)
        {
            var reg = _serviceFactory.GetService<IPaymentSolution>();

            var resp = reg.validateUser(wallet, walletP);

            if (resp.ResCode == ResponseCode.Success)
            {
                var mobile = resp.accntinfo;

                RegisterAccountModel user = new RegisterAccountModel
                {
                    firstname = mobile.fname,
                    middlename = mobile.mname,
                    lastname = mobile.lname,

                    selectedday = Convert.ToDateTime(mobile.birthdate).Day,
                    selectedmonth = Convert.ToDateTime(mobile.birthdate).Month.ToString(),
                    selectedyear = Convert.ToDateTime(mobile.birthdate).Year,
                    selectedgender = mobile.gender,
                    address = mobile.address,
                    mobileno = mobile.mobileno,
                    phoneno = mobile.phoneno,
                    selectedcountry = mobile.country,
                    nationality = mobile.nationality,
                    Wallet = mobile.walletno,
                    Kyc = mobile.kycID,
                    DeviceID = mobile.deviceID,
                    email = mobile.emailadd,
                    isValidMobile = true
                };
                user.month = new[]
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
                user.day = Enumerable.Range(1, 31).Select(x => new SelectListItem
                {
                    Value = x.ToString(CultureInfo.InvariantCulture),
                    Text = x.ToString(CultureInfo.InvariantCulture)
                });

                user.year = Enumerable.Range(1900, (DateTime.Now.Year - 1900) + 1).Reverse().Select(x => new SelectListItem
                {
                    Value = x.ToString(CultureInfo.InvariantCulture),
                    Text = x.ToString(CultureInfo.InvariantCulture)
                });

                TempData["wallet"] = JsonConvert.SerializeObject(user);
                return Json(new { code = resp.ResCode, action = Url.Action("Register", "User") });
            }
            else
            {
                return Json(new { Message = resp.ResMessage });
            }
        }

        [Route("User/Profile")]
        [HttpGet]
        [Authorize]
        public ActionResult UserProfile()
        {
            var srvc = this._serviceFactory.GetService<IPaymentSolution>();
            var res = srvc.getUserInfo("wsclientuser", "wcfw0rdp@ass", User.Identity.Name);

            if (res.ResCode == ResponseCode.Success)
            {
                UserProfileModel user = new UserProfileModel
                {
                    idno = res.userinfo.ID,
                    address = res.userinfo.address,
                    email = res.userinfo.emailadd,
                    firstname = res.userinfo.fname,
                    middlename = res.userinfo.mname,
                    lastname = res.userinfo.lname,
                    mobileno = res.userinfo.mobileno,
                    phoneno = res.userinfo.phoneno,
                    nationality = res.userinfo.nationality,
                    govid2 = res.userinfo.govidno,
                    selectedgovidtype = res.userinfo.govidtype,
                    selectedgender = res.userinfo.gender,
                    selectedcountry = res.userinfo.country,
                    selectedmonth = Convert.ToDateTime(res.userinfo.birthdate).Month.ToString(),
                    selectedday = Convert.ToDateTime(res.userinfo.birthdate).Day,
                    selectedyear = Convert.ToDateTime(res.userinfo.birthdate).Year,

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
                        }),
                };
                return View(user);

            }



            return Json(new { Message = res.ResMessage });
        }

        [Route("User/UpdateProfile")]
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(UserProfileModel data)
        {
            var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));

            data.errors = allErrors;

            if (!ModelState.IsValid)
            {
                return Json(new { code = PayrollCode.ModelError, Message = data.errors });
            }

            var srvc = this._serviceFactory.GetService<IPaymentSolution>();
            UserInfo user = new UserInfo
                {
                    address = data.address,
                    mobileno = data.mobileno,
                    emailadd = data.email,
                    phoneno = data.phoneno,
                    username = User.Identity.Name,
                    ID = data.idno,
                    govidno = data.govid2,
                    govidtype = data.selectedgovidtype
                    //nationality = data.nationality

                };
            var rsp = srvc.updateUserinfo("wsclientuser", "wcfw0rdp@ass", user);
            if (rsp.ResCode == ResponseCode.Success)
            {
                return Json(new { code = rsp.ResCode, Message = rsp.ResMessage });
            }
            else
            {
                return Json(new { code = rsp.ResCode, Message = rsp.ResMessage });
            }
        }

        [AllowAnonymous]
        [Route("ForgotPassword")]
        public ActionResult ForgotPasswordForm()
        {
            return View();
        }
        [AllowAnonymous]
        [Route("Reset")]
        public ActionResult RestPassword(ForgatPasswordModel data)
        {
            return null;
        }
    }
}