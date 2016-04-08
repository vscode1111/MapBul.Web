using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;
using MapBul.Web.Models;

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

        public static string SaveMarkerPhoto(HttpPostedFileBase markerPhoto)
        {
            string virtualPath = "MarkerPhotos/" + Guid.NewGuid() + markerPhoto.FileName.Substring(markerPhoto.FileName.IndexOf(".", StringComparison.Ordinal));
            var siteRoot = HostingEnvironment.MapPath("~/");
            if (siteRoot != null)
            {
                var savePath = Path.Combine(siteRoot, "..", virtualPath);
                markerPhoto.SaveAs(savePath);
            }
            else
                throw new MyException(Errors.UnknownError);
            return virtualPath;
        }

        public static string SaveArticlePhoto(HttpPostedFileBase articlePhoto)
        {
            string virtualPath = "ArticlePhotos/" + Guid.NewGuid() + articlePhoto.FileName.Substring(articlePhoto.FileName.IndexOf(".", StringComparison.Ordinal));
            var siteRoot = HostingEnvironment.MapPath("~/");
            if (siteRoot != null)
            {
                var savePath = Path.Combine(siteRoot, "..", virtualPath);
                articlePhoto.SaveAs(savePath);
            }
            else
                throw new MyException(Errors.UnknownError);
            return virtualPath;
        }
    }
}