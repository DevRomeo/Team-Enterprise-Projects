using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace ML.Web.Controllers
{
    public class CacheManifestHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //don't let the browser/proxies cache the manifest using traditional caching methods.
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();
            context.Response.Cache.SetExpires(DateTime.MinValue);

            //set the correct MIME type for the manifest
            context.Response.ContentType = "text/cache-manifest";

            //manifest requires this on first line
            context.Response.Write("CACHE MANIFEST" + Environment.NewLine);

            //write out the assets that MUST be cached
            context.Response.Write("CACHE:" + Environment.NewLine);

            //write out the links in the bundles
            WriteBundle(context, "~/bundles/javascript");
            WriteBundle(context, "~/css/style");

            //add other assets not mentioned in the bundles
            context.Response.Write(Scripts.Url("~/Images/image1.png") + Environment.NewLine);
            context.Response.Write(Scripts.Url("~/Images/image2.png") + Environment.NewLine);

            //write out the assets that MUST be used online 
            //asterisk(*) means everything that is not already referenced to be cached
            context.Response.Write("NETWORK:" + Environment.NewLine);
            context.Response.Write("*" + Environment.NewLine);

            //if we are debugging then change the manifest file to ensure we download the latest changes
            if (IsDebug) context.Response.Write(Environment.NewLine + DateTime.Now.ToLongTimeString());
        }

        private void WriteBundle(HttpContext context, string virtualPath)
        {

            if (IsDebug)
            {
                foreach (string contentVirtualPath in BundleResolver.Current.GetBundleContents(virtualPath))
                {
                    context.Response.Write(Scripts.Url(contentVirtualPath).ToString() + Environment.NewLine);
                }
            }
            else
            {
                //RELEASE MODE - Url will have cache-busting param added to url
                context.Response.Write(Scripts.Url(virtualPath).ToString() + Environment.NewLine);
            }
        }

        private bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
            return false;
#endif
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

    }
}