
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Controllers;
using System.Web.Mvc;
using System.Web.Security;
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
        private readonly ICustomerPictureService _customerPictureService;
        private readonly ICustomerRepository _customerRepository;
        public CustomerController(IUnitOfWork unitOfWork, 
            [Dependency("MD5")]IEncryptor encryptor, 
            ICustomerPictureService customerPictureService,
            ICustomerRepository customerRepository) : base(unitOfWork)
        {
            _customerRepository = customerRepository;
            _customerPictureService = customerPictureService;
            _encryptor = encryptor;

        }
        public ActionResult List()
        {
            var model = _unitOfWork.GetRepo<Customer>().GetAll().ToList();
            return View(model);
        }

        private class IsActiveModel
        {
            public string Name { get; set; }
            public bool Deger { get; set; }
        }
        public ActionResult Add()
        {
            var customerRole = _unitOfWork.GetRepo<CustomerRole>().GetAll();
            ViewBag.CustomerRoles = new SelectList(customerRole, "Id", "Name");
            var isActiveList = new List<IsActiveModel>
            {
                new IsActiveModel{Name = "Aktif",Deger = true},
                new IsActiveModel{Name = "Pasif",Deger = false}
            };
            ViewBag.isActive = new SelectList(isActiveList, "Deger", "Name");


            return View(new Customer());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(Customer model)
        {
            var validator = new CustomerAddValidator(_customerRepository).Validate(model);
            if (validator.IsValid)
            {
                model.Password = _encryptor.Hash(model.Password);
                model.CreatedTime = DateTime.Now;
                _unitOfWork.GetRepo<Customer>().Add(model);
                var defaultPicture = _customerPictureService.GetDefaultImage();
                var picture = _unitOfWork.GetRepo<Picture>()
                    .Where(x => x.PicturePath == defaultPicture).FirstOrDefault();
                _unitOfWork.GetRepo<CustomerPicture>().Add(new CustomerPicture
                {
                    Customer = model,
                    Picture = picture
                });
            }
            var isSuccess = _unitOfWork.Commit();
            ViewBag.IsSuccess = isSuccess;
            ViewBag.Message = isSuccess ? "Kullanıcı ekleme işlemi başarılı bir şekilde gerçekleştirildi." : "Kullanıcı ekleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            validator.Errors.ToList().ForEach(a =>
                {
                    ModelState.AddModelError(a.PropertyName, a.ErrorMessage);
                });


            var customerRole = _unitOfWork.GetRepo<CustomerRole>().GetAll();
            ViewBag.CustomerRoles = new SelectList(customerRole, "Id", "Name");
            var isActiveList = new List<IsActiveModel>
            {
                new IsActiveModel{Name = "Aktif",Deger = true},
                new IsActiveModel{Name = "Pasif",Deger = false}
            };
            ViewBag.isActive = new SelectList(isActiveList, "Deger", "Name");

            return View();
        }

        #region ProfileEditİşlemleri
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
            var modelCustomerPicture = _unitOfWork.GetRepo<CustomerPicture>().Where(x => x.Customer.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            var isEmailChange = modelCustomerPicture.Customer.Email != model.CustomerPicture.Customer.Email;
            if (validator.IsValid)
            {
                if (modelCustomerPicture != null)
                {
                    modelCustomerPicture.Customer.Name = model.CustomerPicture.Customer.Name;
                    modelCustomerPicture.Customer.Surname = model.CustomerPicture.Customer.Surname;
                    modelCustomerPicture.Customer.Email = model.CustomerPicture.Customer.Email;
                    _unitOfWork.GetRepo<CustomerPicture>().Update(modelCustomerPicture);
                }
            }

            var isSuccess = _unitOfWork.Commit();
            ViewBag.IsSuccess = isSuccess;
            ViewBag.Message = isSuccess ? "Profil güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Profil güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError("CustomerPicture.Customer." + a.PropertyName, a.ErrorMessage);
            });
            model.CustomerPicture = modelCustomerPicture;
            if (!isSuccess || !isEmailChange) return View(model);
            TempData["isEmailChange"] = true;
            TempData["EmailChange"] = "Email Bilgilerinizdeki Değişiklik Sebebiyle Tekrar Giriş Yapmanız Gerekmektedir.";
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");

        }
        [HttpPost]
        public ActionResult PasswordChange(ProfileEditViewModel model)
        {
            var modelCustomerPicture = _unitOfWork.GetRepo<CustomerPicture>().Where(x => x.Customer.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            model.CustomerPicture = modelCustomerPicture;
            if (ModelState.IsValid)
            {
                modelCustomerPicture.Customer.Password = _encryptor.Hash(model.PasswordChangeModel.Password);
                _unitOfWork.GetRepo<Customer>().Update(modelCustomerPicture.Customer);
            }
            var isSuccess = _unitOfWork.Commit();
            ViewBag.IsSuccess = isSuccess;
            ViewBag.Message = isSuccess ? "Şifre güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Şifre güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return View("ProfileEdit", model);
        }
        #endregion

        #region ProfilResmiİşlemleri
        public JsonResult GetProfilPhoto()
        {
            var model = _unitOfWork.GetRepo<CustomerPicture>().Where(x => x.Customer.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            return Json(model.Picture.PicturePath, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ProfilPhotoUpload(HttpPostedFileBase profilphoto)
        {
            if (profilphoto == null) return Json("");


            string uniqueFileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(profilphoto.FileName);
            var picturePath = uniqueFileName + extension;
            var model = _unitOfWork.GetRepo<CustomerPicture>().Where(x => x.Customer.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            model.Picture.PicturePath = picturePath;
            _unitOfWork.GetRepo<CustomerPicture>().Update(model);
            _unitOfWork.Commit();
            var yuklemeYeri = Path.Combine(Server.MapPath("~/Uploads"), picturePath);
            profilphoto.SaveAs(yuklemeYeri);
            //Database e resim ekleme işlemi yap ve eklenen resmin adını döndür
            var isSuccess = _unitOfWork.Commit();
            ViewBag.IsSuccess = isSuccess;
            ViewBag.Message = isSuccess ? "Profil resmi başarılı bir şekilde güncelleştirildi." : "Profil resmi güncelleştirilemedi, lütfen tekrar deneyiniz.";
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
        #endregion


    }
}