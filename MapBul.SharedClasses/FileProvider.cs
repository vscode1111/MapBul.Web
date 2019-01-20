using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using MapBul.SharedClasses.Constants;

namespace MapBul.SharedClasses
{
    public static class FileProvider
    {
        private const string PublicContent = "mapbul.content";
        private const string ArticlePhotos = "ArticlePhotos";
        private const string CategoryIcons = "CategoryIcons";
        private const string MarkerPhotos = "MarkerPhotos";

        public static string GetPublicContent()
        {
            return GetParentDirectoryUntil(PublicContent);
        }

        private static string GetParentDirectoryUntil(string keyWord, string rootDirectory = "")
        {
            if (string.IsNullOrEmpty(rootDirectory))
                rootDirectory = HostingEnvironment.MapPath("~/");
            var dir = Directory.GetParent(rootDirectory);
            if (dir == null) return "";
            var parentDirectory = dir.FullName;
            var needDirectory = $@"{parentDirectory}\{keyWord}";
            return Directory.Exists(needDirectory)
                ? needDirectory
                : GetParentDirectoryUntil(keyWord, parentDirectory);
        }

        private static string SaveFileToPublicContent(HttpPostedFileBase articleTitlePhoto, string imageType, int width = 200, int height = 200)
        {
            var virtualPath =
                $@"{imageType}\{Guid.NewGuid()}{articleTitlePhoto.FileName.Substring(articleTitlePhoto.FileName.IndexOf(".", StringComparison.Ordinal))}";
            var siteRoot = GetPublicContent();
            if (siteRoot != null)
            {
                var savePath = $@"{siteRoot}\{virtualPath}";
                articleTitlePhoto.SaveAs(savePath);
                Image resizedImage;
                using (var image = Image.FromFile(savePath))
                {
                    resizedImage = CompressImage(image, width, height);
                }
                resizedImage.Save(savePath);
            }
            else
                throw new MyException(Errors.UnknownError);
            return virtualPath;
        }

        private static Image CompressImage(Image input, int width, int height)
        {
            var result = new Bitmap(input, width, height);
            return result;
        }

        public static string SaveCategoryIcon(HttpPostedFileBase categoryIcon, int width = 200, int height = 200)
        {
            return SaveFileToPublicContent(categoryIcon, CategoryIcons, width, height);
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
            return SaveFileToPublicContent(markerPhoto, MarkerPhotos);
        }

        public static string[] SaveMarkerPhotos(List<HttpPostedFileBase> markerPhotos)
        {
            var tempStrings = new List<string>();
            var siteRoot = GetPublicContent();
            foreach (var photo in markerPhotos)
            {
                var virtualPath =
                    $@"{MarkerPhotos}\{Guid.NewGuid()}{
                            photo.FileName.Substring(photo.FileName.IndexOf(".", StringComparison.Ordinal))
                        }";
                if (siteRoot != null)
                {
                    var savePath = $"{siteRoot}{virtualPath}";
                    photo.SaveAs(savePath);
                    tempStrings.Add(savePath);
                }
                else
                {
                    throw new MyException(Errors.UnknownError);
                }
            }
            return tempStrings.ToArray();
        }

        public static string SaveMarkerPhoto(byte[] markerPhoto, bool mini)
        {
            var virtualPath = $@"{MarkerPhotos}\{Guid.NewGuid()}.jpg";
            var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            var siteRoot = GetPublicContent();
            if (siteRoot != null)
            {
                var savePath = $@"{siteRoot}\{virtualPath}";
                using (var stream = new MemoryStream(markerPhoto))
                {
                    var myEncoder =
                        Encoder.Quality;
                    var myEncoderParameters = new EncoderParameters(1);
                    var myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    var firstBitmap = new Bitmap(stream);

                    if (Array.IndexOf(firstBitmap.PropertyIdList, 274) > -1)
                    {
                        var orientation = (int)firstBitmap.GetPropertyItem(274).Value[0];
                        switch (orientation)
                        {
                            case 1:
                                // No rotation required.
                                break;
                            case 2:
                                firstBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                break;
                            case 3:
                                firstBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                break;
                            case 4:
                                firstBitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                                break;
                            case 5:
                                firstBitmap.RotateFlip(RotateFlipType.Rotate90FlipX);
                                break;
                            case 6:
                                firstBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                break;
                            case 7:
                                firstBitmap.RotateFlip(RotateFlipType.Rotate270FlipX);
                                break;
                            case 8:
                                firstBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                break;
                        }
                        // This EXIF data is now invalid and should be removed.
                        firstBitmap.RemovePropertyItem(274);
                    }

                    if (!mini)
                    {
                        firstBitmap.Save(savePath);
                    }
                    else
                    {
                        firstBitmap.Save(savePath, jpgEncoder, myEncoderParameters);
                    }
                }
            }
            else
                throw new MyException(Errors.UnknownError);
            return virtualPath;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }

        public static string SaveArticlePhoto(HttpPostedFileBase articlePhoto)
        {
            return SaveFileToPublicContent(articlePhoto, ArticlePhotos);
        }
        
        public static string SaveArticleTitlePhoto(HttpPostedFileBase articleTitlePhoto)
        {
            return SaveFileToPublicContent(articleTitlePhoto, ArticlePhotos);
        }

        public static string SaveMarkerLogo(HttpPostedFileBase markerLogo)
        {
            var virtualPath =
                $@"{MarkerPhotos}\{Guid.NewGuid()}{markerLogo.FileName.Substring(markerLogo.FileName.IndexOf(".", StringComparison.Ordinal))}";
            var siteRoot = GetPublicContent();
            if (siteRoot != null)
            {
                var savePath = $@"{siteRoot}\{virtualPath}";
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
                    var cropRect = new Rectangle(resizedImage.Width > 300 ? (resizedImage.Width - 300)/2 : 0,
                        resizedImage.Height > 300 ? (resizedImage.Height - 300)/2 : 0, 300, 300);

                    cuttedImage = new Bitmap(cropRect.Width, cropRect.Height);

                    using (var g = Graphics.FromImage(cuttedImage))
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
            var virtualPath = $@"{MarkerPhotos}\{Guid.NewGuid()}.jpg";
            var siteRoot = GetPublicContent();
            if (siteRoot != null)
            {
                var savePath = $@"{siteRoot}\{virtualPath}";
                using (var stream = new MemoryStream(markerLogo))
                {
                    var firstBitmap = new Bitmap(stream);

                    if (Array.IndexOf(firstBitmap.PropertyIdList, 274) > -1)
                    {
                        var orientation = (int)firstBitmap.GetPropertyItem(274).Value[0];
                        switch (orientation)
                        {
                            case 1:
                                // No rotation required.
                                break;
                            case 2:
                                firstBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                break;
                            case 3:
                                firstBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                break;
                            case 4:
                                firstBitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                                break;
                            case 5:
                                firstBitmap.RotateFlip(RotateFlipType.Rotate90FlipX);
                                break;
                            case 6:
                                firstBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                break;
                            case 7:
                                firstBitmap.RotateFlip(RotateFlipType.Rotate270FlipX);
                                break;
                            case 8:
                                firstBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                break;
                        }
                        // This EXIF data is now invalid and should be removed.
                        firstBitmap.RemovePropertyItem(274);
                    }

                    firstBitmap.Save(savePath);
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
                    var cropRect = new Rectangle(resizedImage.Width > 300 ? (resizedImage.Width - 300) / 2 : 0,
                        resizedImage.Height > 300 ? (resizedImage.Height - 300) / 2 : 0, 300, 300);

                    cuttedImage = new Bitmap(cropRect.Width, cropRect.Height);

                    using (var g = Graphics.FromImage(cuttedImage))
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