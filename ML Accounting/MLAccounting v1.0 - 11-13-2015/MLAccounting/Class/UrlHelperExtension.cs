using System;
using System.Web;
using System.Web.Mvc;

namespace Accounting
{
    public static class UrlHelperExtension
    {
        public static string Absolute(this UrlHelper url, string relativeOrAbsolute)
        {
            var uri = new Uri(relativeOrAbsolute, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri)
            {
                return relativeOrAbsolute;
            }
            // At this point, we know the url is relative.
            return VirtualPathUtility.ToAbsolute(relativeOrAbsolute);
        }
        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            string newUrl = serverUrl;
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                "://" + originalUri.Authority + newUrl;
            return newUrl;
        } 
    }

}