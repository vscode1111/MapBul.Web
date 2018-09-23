using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
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
            var virtualPath = "CategoryIcons\\" + Guid.NewGuid() + categoryIcon.FileName.Substring(categoryIcon.FileName.IndexOf(".", StringComparison.Ordinal));
            var siteRoot = HostingEnvironment.MapPath("~/");
            if (siteRoot != null)
            {
                //var savePath = Path.Combine(siteRoot, "..", virtualPath);
                var savePath = $"{siteRoot}{virtualPath}";
                var dirPath = Path.GetDirectoryName(savePath);
                if (!string.IsNullOrEmpty(dirPath) && !Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
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
            var virtualPath = "MarkerPhotos\\" + Guid.NewGuid() + markerPhoto.FileName.Substring(markerPhoto.FileName.IndexOf(".", StringComparison.Ordinal));
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

        public static string[] SaveMarkerPhotos(List<HttpPostedFileBase> markerPhotos)
        {
            var tempStrings = new List<string>();
            foreach (var photo in markerPhotos)
            {
                var virtualPath = "MarkerPhotos/" + Guid.NewGuid() + photo.FileName.Substring(photo.FileName.IndexOf(".", StringComparison.Ordinal));
                var siteRoot = HostingEnvironment.MapPath("~/");
                if (siteRoot != null)
                {
                    var savePath = Path.Combine(siteRoot, "..", virtualPath);
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
            var virtualPath = "MarkerPhotos/" + Guid.NewGuid() + ".jpg";
            var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            var siteRoot = HostingEnvironment.MapPath("~/");
            if (siteRoot != null)
            {
                var savePath = Path.Combine(siteRoot, "..", virtualPath);
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





                    /*var newItem = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                    newItem.Id = 274;
                    newItem.Value = new byte[] { 1 };
                    firstBitmap.SetPropertyItem(newItem);*/
                    if (!mini)
                    {
                        firstBitmap.Save(savePath);
                    }
                    else
                    {
                        firstBitmap.Save(savePath, jpgEncoder, myEncoderParameters);
                    }
                }

                /*using (var imageFile = new FileStream(savePath, FileMode.Create))
                {
                    imageFile.Write(markerPhoto, 0, markerPhoto.Length);
                    imageFile.Flush();
                }*/
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
            var virtualPath = "ArticlePhotos\\" + Guid.NewGuid() + articlePhoto.FileName.Substring(articlePhoto.FileName.IndexOf(".", StringComparison.Ordinal));
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
            var virtualPath = "ArticlePhotos\\" + Guid.NewGuid() + articleTitlePhoto.FileName.Substring(articleTitlePhoto.FileName.IndexOf(".", StringComparison.Ordinal));
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
            var virtualPath = "MarkerPhotos/" + Guid.NewGuid() +
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
            var virtualPath = "MarkerPhotos/" + Guid.NewGuid() + ".jpg";
            var siteRoot = HostingEnvironment.MapPath("~/");
            if (siteRoot != null)
            {
                var savePath = Path.Combine(siteRoot, "..", virtualPath);
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





                    /*var newItem = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                    newItem.Id = 274;
                    newItem.Value = new byte[] { 1 };
                    firstBitmap.SetPropertyItem(newItem);*/
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