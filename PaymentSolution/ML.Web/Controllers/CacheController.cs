using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ML.Web.Models;
using System.Text;
using System.Web.Optimization;
using System.IO;

namespace ML.Web.Controllers
{
    public class CacheController : Controller
    {
        //
        // GET: /Cache/
        public ActionResult AppManifest()
        {
            var m = new CacheManifestModel();
            m.AssemblyVersion = GetType().Assembly.GetName().Version.ToString();
            m.CacheCollection.Add(WriteBundle("~/bundles/jquery"));
            m.CacheCollection.Add(WriteBundle("~/bundles/AccountRegistration"));

            m.CacheCollection.Add(GetPhysicalFileToCache("~/css", "style*.css", ""));
            return View();
        }

        private string WriteBundle(string virtualPath)
        {
            var bundleString = new StringBuilder();
            bundleString.Append(Scripts.Url(virtualPath));
            return bundleString.ToString();
        }

        private string GetPhysicalFileToCache(string relativeFolderToAssets, string fileType, string cdnBucket)
        {
            var outputString = new StringBuilder();
            var folder = new DirectoryInfo(Server.MapPath(relativeFolderToAssets));

            foreach (FileInfo file in folder.GetFiles(fileType))
            {
                string locaion = !string.IsNullOrEmpty(cdnBucket) ? cdnBucket : relativeFolderToAssets;
                string outputFileName = (locaion + "/" + file).Replace("~", "");
                outputString.AppendLine(outputFileName);
            }
            return outputString.ToString();
        }
    }
}