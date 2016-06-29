using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Hosting;
using MapBul.SharedClasses.Constants;

namespace MapBul.SharedClasses
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

        public static string SaveMarkerPhoto(byte[] markerPhoto)
        {
            string virtualPath = "MarkerPhotos/" + Guid.NewGuid() + ".jpg";
            var siteRoot = HostingEnvironment.MapPath("~/");
            if (siteRoot != null)
            {
                var savePath = Path.Combine(siteRoot, "..", virtualPath);
                using (var imageFile = new FileStream(savePath, FileMode.Create))
                {
                    imageFile.Write(markerPhoto, 0, markerPhoto.Length);
                    imageFile.Flush();
                }
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
            string virtualPath = "MarkerPhotos/" + Guid.NewGuid() +
                                 markerLogo.FileName.Substring(markerLogo.FileName.IndexOf(".", StringComparison.Ordinal));
            var siteRoot = HostingEnvironment.MapPath("~/");
            if (siteRoot != null)
            {
                var savePath = Path.Combine(siteRoot, "..", virtualPath);
                markerLogo.SaveAs(savePath);
                Bitmap cuttedImage;
                using (var image = Image.FromFile(savePath))
                {
                    float k = 0;
                    if (image.Width < image.Height)
                        k = 300f/image.Width;
                    if (image.Width >= image.Height)
                        k = 300f/image.Height;
                    var resizedImage = CompressImage(image, (int) (k*image.Width), (int) (k*image.Height));
                    Rectangle cropRect = new Rectangle(resizedImage.Width > 300 ? (resizedImage.Width - 300)/2 : 0,
                        resizedImage.Height > 300 ? (resizedImage.Height - 300)/2 : 0, 300, 300);

                    cuttedImage = new Bitmap(cropRect.Width, cropRect.Height);

                    using (Graphics g = Graphics.FromImage(cuttedImage))
                    {
                        g.DrawImage(resizedImage, new Rectangle(0, 0, cuttedImage.Width, cuttedImage.Height),
                            cropRect,
                            GraphicsUnit.Pixel);
                    }
                }
                cuttedImage.Save(savePath);
            }
            else
                throw new MyException(Errors.UnknownError);
            return virtualPath;
        }

        public static string SaveMarkerLogo(byte[] markerLogo)
        {
            string virtualPath = "MarkerPhotos/" + Guid.NewGuid() + ".jpg";
            var siteRoot = HostingEnvironment.MapPath("~/");
            if (siteRoot != null)
            {
                var savePath = Path.Combine(siteRoot, "..", virtualPath);
                using (var imageFile = new FileStream(savePath, FileMode.Create))
                {
                    imageFile.Write(markerLogo, 0, markerLogo.Length);
                    imageFile.Flush();
                }
                Bitmap cuttedImage;
                using (var image = Image.FromFile(savePath))
                {
                    float k = 0;
                    if (image.Width < image.Height)
                        k = 300f / image.Width;
                    if (image.Width >= image.Height)
                        k = 300f / image.Height;
                    var resizedImage = CompressImage(image, (int)(k * image.Width), (int)(k * image.Height));
                    Rectangle cropRect = new Rectangle(resizedImage.Width > 300 ? (resizedImage.Width - 300) / 2 : 0,
                        resizedImage.Height > 300 ? (resizedImage.Height - 300) / 2 : 0, 300, 300);

                    cuttedImage = new Bitmap(cropRect.Width, cropRect.Height);

                    using (Graphics g = Graphics.FromImage(cuttedImage))
                    {
                        g.DrawImage(resizedImage, new Rectangle(0, 0, cuttedImage.Width, cuttedImage.Height),
                            cropRect,
                            GraphicsUnit.Pixel);
                    }
                }
                cuttedImage.Save(savePath);
            }
            else
                throw new MyException(Errors.UnknownError);
            return virtualPath;
        }
    }
}