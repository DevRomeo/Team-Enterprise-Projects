using System.Web;
using System.Web.Mvc;
using StructureMap.Web.Pipeline;

namespace ML.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            StructureMapConfig.Configure();
            AreaRegistration.RegisterAllAreas();
        }

        protected void Application_EndRequest()
        {
            HttpContextLifecycle.DisposeAndClearAll();
            var context = new HttpContextWrapper(Context);
            if (Context.Response.StatusCode == 302 && context.Request.IsAjaxRequest())
            {                
                Context.Response.Clear();
                Context.Response.StatusCode = 401;                
            }
        }
    }
}
