using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Hosting;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;

namespace MapBul.Web.FileProvider
{
    public static class FileProvider
    {
        private static Image CompressImage(Image input, int width, int height)
        {
            var result = new Bitmap(input, width, height);
            return result;
        }
        public static string SaveCategoryIcon(HttpPostedFileBase categoryIcon)
        {
            string virtualPath = "CategoryIcons/" + Guid.NewGuid() + categoryIcon.FileName.Substring(categoryIcon.FileName.IndexOf(".", StringComparison.Ordinal));
            var siteRoot=HostingEnvironment.MapPath("~/");
            if (siteRoot != null)
            {
                var savePath = Path.Combine(siteRoot, "..", virtualPath);
                categoryIcon.SaveAs(savePath);
                Image resizedImage;
                using (var image = Image.FromFile(savePath))
                {
                    resizedImage = CompressImage(image, 120, 120);
                }
                resizedImage.Save(savePath);
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
                try
                {
                    var absolutePath = Path.Combine(siteRoot, "..", path);
                    File.Delete(absolutePath);
                }
                catch
                {
                    // ignored
                }
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
            string virtualPath = "ArticlePhotos\\" + Guid.NewGuid() + articlePhoto.FileName.Substring(articlePhoto.FileName.IndexOf(".", StringComparison.Ordinal));
            var siteRoot = HostingEnvironment.MapPath("~/");
            if (siteRoot != null)
            {
                var savePath = Path.Combine(siteRoot, "..", virtualPath);
                articlePhoto.SaveAs(savePath);
                Image resizedImage;
                using (var image = Image.FromFile(savePath))
                {
                    resizedImage = CompressImage(image, 600, 600);
                }
                resizedImage.Save(savePath);
            }
            else
                throw new MyException(Errors.UnknownError);
            return virtualPath;
        }

        public static string SaveArticleTitlePhoto(HttpPostedFileBase articleTitlePhoto)
        {
            string virtualPath = "ArticlePhotos\\" + Guid.NewGuid() + articleTitlePhoto.FileName.Substring(articleTitlePhoto.FileName.IndexOf(".", StringComparison.Ordinal));
            var siteRoot = HostingEnvironment.MapPath("~/");
            if (siteRoot != null)
            {
                var savePath = Path.Combine(siteRoot, "..", virtualPath);
                articleTitlePhoto.SaveAs(savePath);
                Image resizedImage;
                using (var image = Image.FromFile(savePath))
                {
                    resizedImage = CompressImage(image, 200, 200);
                }
                resizedImage.Save(savePath);
            }
            else
                throw new MyException(Errors.UnknownError);
            return virtualPath;
        }

        public static string SaveMarkerLogo(HttpPostedFileBase markerLogo)
        {
            string virtualPath = "MarkerPhotos/" + Guid.NewGuid() + markerLogo.FileName.Substring(markerLogo.FileName.IndexOf(".", StringComparison.Ordinal));
            var siteRoot = HostingEnvironment.MapPath("~/");
            if (siteRoot != null)
            {
                var savePath = Path.Combine(siteRoot, "..", virtualPath);
                markerLogo.SaveAs(savePath);
                Image resizedImage;
                using (var image = Image.FromFile(savePath))
                {
                    resizedImage = CompressImage(image, 300, 300);
                }
                resizedImage.Save(savePath);
            }
            else
                throw new MyException(Errors.UnknownError);
            return virtualPath;
        }
    }
}