
using System.IO;
using System.Linq;
using System.Web;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Controllers;
using System.Web.Mvc;
using FashionStore.Entity.Entities;
using FashionStore_BLL.Services.Abstracts;
using FashionStore_BLL.Services.Validations;
using FluentValidation.Results;
using Unity.Attributes;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class CustomerController : BaseController
    {
        private readonly IEncryptor _encryptor;
        public CustomerController(IUnitOfWork unitOfWork, [Dependency("MD5")]IEncryptor encryptor) : base(unitOfWork)
        {
            _encryptor = encryptor;
        }
        public ActionResult List()
        {
            var model = _unitOfWork.GetRepo<Customer>().GetAll().ToList();
            return View(model);
        }

        public ActionResult ProfileEdit()
        {
            var model = _unitOfWork.GetRepo<CustomerPicture>().Where(x => x.Customer.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult ProfileEdit(CustomerPicture model)
        {

            var validator = new ProfilUpdateValidator();
            ValidationResult result = validator.Validate(model);
            if (result.IsValid)
            {
                model.Customer.Password = _encryptor.Hash(model.Customer.Password);
                _unitOfWork.GetRepo<CustomerPicture>().Update(model);
                _unitOfWork.Commit();
            }
            //var m = new MessageResult
            //{
            //    ErrorMessage = result.Errors.Select(x => x.ErrorMessage).ToList(),
            //    IsSucceed = result.IsValid
            //};
            //m.SuccessMessage = m.IsSucceed == true ? "." : "Hatalı bilgiler mevcut";



            return RedirectToAction("ProfileEdit");
        }

        public JsonResult GetProfilPhoto()
        {
            var model = _unitOfWork.GetRepo<CustomerPicture>().Where(x => x.Customer.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            return Json(model.Picture.PicturePath, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ProfilPhotoUpload(HttpPostedFileBase profilphoto)
        {
            if (profilphoto == null) return Json("");

            var picturePath = Path.GetFileName(profilphoto.FileName);
            var model = _unitOfWork.GetRepo<CustomerPicture>().Where(x => x.Customer.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            model.Picture.PicturePath = picturePath;
            _unitOfWork.GetRepo<CustomerPicture>().Update(model);
            _unitOfWork.Commit();
            var yuklemeYeri = Path.Combine(Server.MapPath("~/Uploads"), picturePath);
            profilphoto.SaveAs(yuklemeYeri);
            //Database e resim ekleme işlemi yap ve eklenen resmin adını döndür
            ViewBag.Kontrol = true;
            return Json(picturePath);
        }
        public JsonResult ProfilPhotoDelete()
        {
            var model = _unitOfWork.GetRepo<CustomerPicture>().Where(x => x.Customer.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            var fullPath = Server.MapPath("~/Uploads/" + model.Picture.PicturePath);

            if (model.Picture.PicturePath == "defaultprofil.jpg")
                return Json(model.Picture.PicturePath, JsonRequestBehavior.AllowGet);

            if (!System.IO.File.Exists(fullPath)) return Json(model.Picture.PicturePath, JsonRequestBehavior.AllowGet);

            System.IO.File.Delete(fullPath);
            model.Picture.PicturePath = "defaultprofil.jpg";
            _unitOfWork.GetRepo<CustomerPicture>().Update(model);
            _unitOfWork.Commit();
            return Json(model.Picture.PicturePath, JsonRequestBehavior.AllowGet);
        }
        

    }
}