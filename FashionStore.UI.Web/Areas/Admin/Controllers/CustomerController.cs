
using System.IO;
using System.Linq;
using System.Web;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Controllers;
using System.Web.Mvc;
using FashionStore.Entity.Entities;
using FashionStore.UI.Web.Areas.Admin.Models;
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
            var model = new ProfileEditViewModel()
            {
                CustomerPicture = _unitOfWork.GetRepo<CustomerPicture>()
                    .Where(x => x.Customer.Email == HttpContext.User.Identity.Name).FirstOrDefault()
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ProfileEdit(ProfileEditViewModel model)
        {
            var validator = new ProfilUpdateValidator(_unitOfWork).Validate(model.CustomerPicture.Customer);
            if (validator.IsValid)
            {
                var modelCustomer = _unitOfWork.GetRepo<Customer>().Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault();

                if (modelCustomer != null)
                {
                    modelCustomer.Name = model.CustomerPicture.Customer.Name;
                    modelCustomer.Surname = model.CustomerPicture.Customer.Surname;
                    modelCustomer.Email = model.CustomerPicture.Customer.Email;
                    _unitOfWork.GetRepo<Customer>().Update(modelCustomer);
                }

                var isSuccess = _unitOfWork.Commit();
                ViewBag.IsSuccess = isSuccess;
                ViewBag.Message = isSuccess ? "Profil güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Profil güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
                model.CustomerPicture.Customer = modelCustomer;
            }
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError("CustomerPicture.Customer."+a.PropertyName, a.ErrorMessage);
            });
            return View(model);
        }
        [HttpPost]
        public ActionResult PasswordChange(ProfileEditViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("ProfileEdit");

            var modelCustomer = _unitOfWork.GetRepo<Customer>().Where(x => x.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            if (modelCustomer != null)
            {
                //modelCustomer.Password = _encryptor.Hash(model.Password);
                _unitOfWork.GetRepo<Customer>().Update(modelCustomer);
            }

            var isSuccess = _unitOfWork.Commit();
            ViewBag.IsSuccess = isSuccess;
            ViewBag.Message = isSuccess ? "Şifre güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Şifre güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
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