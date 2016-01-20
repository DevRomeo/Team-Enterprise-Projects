using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AccountingWeb;
using System.Web.Security;
using MLAccountingWeb.Security;
using Newtonsoft.Json;

namespace MLAccountingWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        public void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                CustomPrincipalSerializeModel userInfo = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                CustomPrincipal newUser = new CustomPrincipal(authTicket.Name)
                {
                    FirstName = userInfo.FirstName,
                    MiddleName = userInfo.MiddleName,
                    roles = userInfo.roles,
                    LastName = userInfo.LastName,
                    UserId = userInfo.UserId
                };
                HttpContext.Current.User = newUser;
            }
        }

        protected void Application_EndRequest()
        {
            var context = new HttpContextWrapper(Context);
            if (Context.Response.StatusCode == 302 && context.Request.IsAjaxRequest())
            {
                Context.Response.Clear();
                Context.Response.StatusCode = 401;
            }
        }
    }
}
