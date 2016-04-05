using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;

namespace MapBul.Web.FileProvider
{
    public static class FileProvider
    {
        public static string SaveCategoryIcon(HttpPostedFileBase categoryIcon)
        {

            string virtualPath = "CategoryIcons/" + Guid.NewGuid() + categoryIcon.FileName.Substring(categoryIcon.FileName.IndexOf(".", StringComparison.Ordinal));
            var siteRoot=HostingEnvironment.MapPath("~/");
            if (siteRoot != null)
            {
                var savePath = Path.Combine(siteRoot, "..", virtualPath);
                categoryIcon.SaveAs(savePath);
            }
            else 
                throw new MyException(Errors.UnknownError);
            return virtualPath;
        }

        public static void DeleteFile(string path)
        {
            var siteRoot = HostingEnvironment.MapPath("~/");
            if (siteRoot != null)
            {
                var absolutePath = Path.Combine(siteRoot, "..", path);
                File.Delete(absolutePath);
            }
            else 
                throw new MyException(Errors.UnknownError);
        }
    }
}