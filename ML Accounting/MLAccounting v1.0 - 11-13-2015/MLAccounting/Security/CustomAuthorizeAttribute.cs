using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MLAccountingWeb.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        #region Properties
        public string UserConfigKey { get; set; }
        public string RolesConfigKey { get; set; }
        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }
        #endregion

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                var authorizedUsers = ConfigurationManager.AppSettings[UserConfigKey];
                var authorizedRoles = ConfigurationManager.AppSettings[RolesConfigKey];

                Users = String.IsNullOrEmpty(Users) ? authorizedUsers : Users;
                Roles = string.IsNullOrEmpty(Roles) ? authorizedRoles : Roles;

                if (!String.IsNullOrEmpty(Roles))
                {
                    if (!CurrentUser.IsInRole(Roles))
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(new { controller = "Errors", action = "UnAuthorizedUser" }));
                    }
                }

                if (!String.IsNullOrEmpty(Users))
                {
                    if (!Users.Contains(CurrentUser.UserId.ToString()))
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(new { controller = "Errors", action = "InvalidUser" }));
                    }
                }
            }
            //base.OnAuthorization(filterContext);
        }
    }
}