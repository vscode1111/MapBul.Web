using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MapBul.Web.Controllers
{
    public class ArticlePhotoController : Controller
    {
        public class StreamResult : ViewResult
        {
            public Stream Stream { get; set; }
            public string ContentType { get; set; }
            public string ETag { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.ContentType = ContentType;
                if (ETag != null) context.HttpContext.Response.AddHeader("ETag", ETag);
                const int size = 4096;
                byte[] bytes = new byte[size];
                int numBytes;
                while ((numBytes = Stream.Read(bytes, 0, size)) > 0)
                    context.HttpContext.Response.OutputStream.Write(bytes, 0, numBytes);
                Stream.Close();
                Stream.Dispose();
            }
        }


        public StreamResult Index(string fileName)
        {
            var t = fileName;
            var file =
                @"D:\_Cloud\_Projects\Scub111\X-island\MBWEB_GIT\MBWEB_GIT\MapBul.Web\ArticlePhotos\127bac1d-aac3-4f52-8492-b1f7774ef1e9.png";
            var fileNameInt = Path.GetFileName(file);
            var type = MimeMapping.GetMimeMapping(fileNameInt);
            //return File(file, type, fileNameInt);
            /*
            byte[] imageData = System.IO.File.ReadAllBytes(file);
            Response.ContentType = type;
            Response.BinaryWrite(imageData);
            */
            var fileStream = new FileStream(file, FileMode.Open);
            return new StreamResult {Stream = fileStream, ContentType = type, ETag = "scub111"};
        }
    }
}