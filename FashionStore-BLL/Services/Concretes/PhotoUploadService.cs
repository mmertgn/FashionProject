using FashionStore_BLL.Services.Abstracts;
using System;
using System.IO;
using System.Web;

namespace FashionStore_BLL.Services.Concretes
{
    public class PhotoUploadService : IUploadService
    {
        public string Upload(HttpPostedFileBase file)
        {
            string uniqueFileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(file.FileName);
            var picturePath = uniqueFileName + extension;

            var yuklemeYeri = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads"), picturePath);
            file.SaveAs(yuklemeYeri);
            return picturePath;
        }
    }
}
