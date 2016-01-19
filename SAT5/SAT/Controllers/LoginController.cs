using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAT.Models;
using SAT.Util;
using System.Web.Security;
namespace SAT.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index() 
        {
            return RedirectToAction("Login");
        }
        public ActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel user)
        {
            if (ModelState.IsValid) 
            {
                user.id = 12345;
                MySession.userID = user.id;
                FormsAuthentication.SetAuthCookie(user.userName, false);
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }
        [Authorize]
        public ActionResult LogOff() 
        {
            FormsAuthentication.SignOut();
            MySession.userID = -1;
            return RedirectToAction("Login");
        } 
    }
}