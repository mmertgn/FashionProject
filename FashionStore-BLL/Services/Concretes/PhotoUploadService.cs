using FashionStore_BLL.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using ImageResizer;

namespace FashionStore_BLL.Services.Concretes
{
    public class PhotoUploadService : IUploadService
    {
        public string Upload(HttpPostedFileBase file,
            int width, int height)
        {
            string uniqueFileName = Guid.NewGuid().ToString();
            var versions = new Dictionary<string, string>();
            versions.Add("_small", "maxwidth="+width+"&maxheight="+height+"&format=jpg");
            var yuklemeYeri = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads"), uniqueFileName);
            ImageBuilder.Current.Build(
                new ImageJob(
                    file.InputStream,
                    yuklemeYeri,
                    new Instructions(versions["_small"]),
                    false,
                    true));
            return uniqueFileName + ".jpg";
        }
    }
}
