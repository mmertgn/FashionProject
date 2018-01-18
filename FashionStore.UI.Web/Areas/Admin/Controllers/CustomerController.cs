
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Controllers;
using System.Web.Mvc;
using System.Web.Routing;
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
        private readonly IUploadService _uploadService;
        public CustomerController(IUnitOfWork unitOfWork,
            [Dependency("MD5")]IEncryptor encryptor,
            ICustomerPictureService customerPictureService,
            ICustomerRepository customerRepository,
            [Dependency("PhotoUpload")]IUploadService uploadService) : base(unitOfWork)
        {
            _customerRepository = customerRepository;
            _customerPictureService = customerPictureService;
            _encryptor = encryptor;
            _uploadService = uploadService;

        }
        public ActionResult List()
        {
            var model = _unitOfWork.GetRepo<Customer>().Where(x=>x.Deleted==false).ToList();
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

        public ActionResult Edit(int id)
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);

            var customerRole = _unitOfWork.GetRepo<CustomerRole>().GetAll();
            ViewBag.CustomerRoles = new SelectList(customerRole, "Id", "Name");
            var isActiveList = new List<IsActiveModel>
            {
                new IsActiveModel{Name = "Aktif",Deger = true},
                new IsActiveModel{Name = "Pasif",Deger = false}
            };
            ViewBag.isActive = new SelectList(isActiveList, "Deger", "Name");

            var model = new CustomerEditViewModel()
            {
                Customer = _unitOfWork.GetRepo<Customer>()
                    .Where(x => x.Id == id).FirstOrDefault(),
                Addresses = _unitOfWork.GetRepo<Address>().Where(x => x.CustomerId == id).ToList()
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerEditViewModel model)
        {
            var validator = new CustomerEditValidator(_customerRepository).Validate(model.Customer);
            var modelCustomer = _unitOfWork.GetRepo<Customer>().Where(x => x.Id == model.Customer.Id).FirstOrDefault();
            if (validator.IsValid)
            {
                modelCustomer.Name = model.Customer.Name;
                modelCustomer.Surname = model.Customer.Surname;
                modelCustomer.Email = model.Customer.Email;
                modelCustomer.Active = model.Customer.Active;
                modelCustomer.CustomerRoleId = model.Customer.CustomerRoleId;
                _unitOfWork.GetRepo<Customer>().Update(modelCustomer);
            }

            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError("Customer." + a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Kullanıcı bilgileri güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Kullanıcı bilgileri güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";

            return RedirectToAction("Edit", new { id = modelCustomer.Id });
        }

        public ActionResult CustomerDelete(int id)
        {
            var modelCustomer = _unitOfWork.GetRepo<Customer>().Where(x => x.Id == id).FirstOrDefault();
            modelCustomer.Deleted = true;
            _unitOfWork.GetRepo<Customer>().Update(modelCustomer);
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            TempData["Message"] = isSuccess ? "Kullanıcı silme işlemi başarılı bir şekilde gerçekleştirildi." : "Kullanıcı silme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("List");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CustomerPasswordChange(CustomerEditViewModel model, int CustomerId)
        {
            var modelCustomer = _unitOfWork.GetRepo<Customer>().Where(x => x.Id == CustomerId).FirstOrDefault();
            model.Customer = modelCustomer;
            if (ModelState.IsValid)
            {
                modelCustomer.Password = _encryptor.Hash(model.PasswordChangeModel.Password);
                _unitOfWork.GetRepo<Customer>().Update(modelCustomer);
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Şifre güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Şifre güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";

            return RedirectToAction("Edit", new { id = CustomerId });
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult AddressAdd(CustomerEditViewModel model)
        {
            var validator = new AddressAddValidator().Validate(model.Address);
            if (validator.IsValid)
            {
                model.Address.CreatedOnUtc = DateTime.Now;
                _unitOfWork.GetRepo<Address>().Add(model.Address);
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            TempData["Message"] = isSuccess ? "Adres ekleme işlemi başarılı bir şekilde gerçekleştirildi." : "Adres ekleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError("Address."+a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;

            return RedirectToAction("Edit", new { id = model.Address.CustomerId });
        }


        public ActionResult AddressEdit(int id)
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);

            var address = _unitOfWork.GetRepo<Address>().Where(x => x.Id == id).FirstOrDefault();

            return View(address);
        }
        [HttpPost]
        public ActionResult AddressEdit(Address address)
        {
            var validator = new AddressAddValidator().Validate(address);
            var modelAddress = _unitOfWork.GetRepo<Address>().Where(x => x.Id == address.Id).FirstOrDefault();
            if (validator.IsValid)
            {
                modelAddress.CustomerId = address.CustomerId;
                modelAddress.FirstName = address.FirstName;
                modelAddress.LastName = address.LastName;
                modelAddress.Email = address.Email;
                modelAddress.PhoneNumber = address.PhoneNumber;
                modelAddress.PostalCode = address.PostalCode;
                modelAddress.Town = address.Town;
                modelAddress.City = address.City;
                modelAddress.Address1 = address.Address1;
                modelAddress.Address2 = address.Address2;
                _unitOfWork.GetRepo<Address>().Update(modelAddress);
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError(a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Adres bilgileri güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Adres bilgileri güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";

            return RedirectToAction("AddressEdit",new{id=address.Id});
        }

        public ActionResult AddressDelete(int id)
        {
            var address = _unitOfWork.GetRepo<Address>().Where(x => x.Id == id).FirstOrDefault();
            _unitOfWork.GetRepo<Address>().Delete(id);
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            TempData["Message"] = isSuccess ? "Adres silme işlemi başarılı bir şekilde gerçekleştirildi." : "Adres silme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("Edit", new { id = address.CustomerId });
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

            var model = _unitOfWork.GetRepo<CustomerPicture>().Where(x => x.Customer.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            var picturePath = _uploadService.Upload(profilphoto);
            model.Picture.PicturePath = picturePath;
            _unitOfWork.GetRepo<CustomerPicture>().Update(model);

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