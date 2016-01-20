using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLAccountingWeb.Controllers
{    
    public class ErrorsController : Controller
    {
        //
        // GET: /Errors/
        [Route("InvalidUser")]
        public ActionResult InvalidUser()
        {
            return PartialView("_InvalidUser");
        }
        [Route("UnAuthorized")]
        public ActionResult UnAuthorizedUser()
        {
            return PartialView("_UNAuthorizedUser");
        }
    }
}