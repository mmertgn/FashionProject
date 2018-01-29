using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Models;
using FashionStore_BLL.Services.Abstracts;
using FashionStore_BLL.Validations.CustomerValidations;
using FashionStore_BLL.Validations.PasswordValidations;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using FashionStore_BLL.Services.Concretes;
using FashionStore_BLL.Validations.ProfilValidations;
using Unity.Attributes;

namespace FashionStore.UI.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IEncryptor _encryptor;
        private readonly IPictureService _customerPictureService;
        private readonly IMessaging _messaging;
        // GET: Account
        public AccountController(IUnitOfWork unitOfWork,
            [Dependency("MD5")]IEncryptor encryptor,
            IMessaging messaging,
            [Dependency("CustomerPicture")]IPictureService customerPictureService) : base(unitOfWork)
        {
            _encryptor = encryptor;
            _messaging = messaging;
            _customerPictureService = customerPictureService;
        }
        [CustomAuthorization(Roles = "Üye,Admin")]
        public ActionResult MyAccount()
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);

            var user = _unitOfWork.GetRepo<Customer>().GetObject(x => x.Email == User.Identity.Name);
            var adresses = _unitOfWork.GetRepo<Address>().Where(x => x.Customer.Email == User.Identity.Name)
                .ToList();
            var shipAddressId = user.ShippingAddress?.Id ?? 0;
            var billAddressId = user.BillingAddress?.Id ?? 0;
            ViewBag.ShippingAdresses = new SelectList(adresses, "Id", "Address1", shipAddressId);
            ViewBag.BillingAdresses = new SelectList(adresses, "Id", "Address1", billAddressId);
            var model = new ProfileViewModel
            {
                Customer = _unitOfWork.GetRepo<Customer>().GetObject(x => x.Email == User.Identity.Name)
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [CustomAuthorization(Roles = "Üye,Admin")]
        public ActionResult ProfileEdit(ProfileViewModel model)
        {
            var user = _unitOfWork.GetRepo<Customer>().GetObject(x => x.Email == User.Identity.Name);
            var IsEmailChanged = model.Customer.Email != user.Email;
            user.Name = model.Customer.Name;
            user.Surname = model.Customer.Surname;
            user.Email = model.Customer.Email;
            user.DateOfBirth = model.Customer.DateOfBirth;
            user.BillingAddress =
                _unitOfWork.GetRepo<Address>().GetObject(x => x.Id == model.Customer.BillingAddress.Id);
                user.ShippingAddress = _unitOfWork.GetRepo<Address>().GetObject(x => x.Id == model.Customer.ShippingAddress.Id);
            var validator = new ProfilUpdateValidator(_unitOfWork).Validate(user);
            if (validator.IsValid)
            {
                _unitOfWork.GetRepo<Customer>().Update(user);
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError("Customer."+a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Kullanıcı bilgileri güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Kullanıcı bilgileri güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            if (IsEmailChanged && isSuccess)
            {
                FormsAuthentication.SignOut();
                TempData["Message"] = "Güncelleme işlemi başarıyla gerçekleştirildi.Email adresinizi değiştirdiğiniz için tekrar giriş yapmanız gerekmektedir.";
                return RedirectToAction("SignIn");
            }
            return RedirectToAction("MyAccount");
        }
        [HttpPost, ValidateAntiForgeryToken]
        [CustomAuthorization(Roles = "Üye,Admin")]
        public ActionResult PasswordEdit(ProfileViewModel model)
        {
            var modelCustomer = new Customer
            {
                Password = model.Password
            };
            var validator = new PasswordResetValidator(model.PasswordConfirm).Validate(modelCustomer);
            var user = _unitOfWork.GetRepo<Customer>().GetObject(x => x.Email == User.Identity.Name);
            if (validator.IsValid)
            {
                user.Password = _encryptor.Hash(model.Password);
                _unitOfWork.GetRepo<Customer>().Update(user);
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError(a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Şifre güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Şifre güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("MyAccount");
        }
        [HttpPost, ValidateAntiForgeryToken]
        [CustomAuthorization(Roles = "Üye,Admin")]
        public ActionResult AddressAdd(ProfileViewModel model)
        {
            var validator = new AddressAddValidator().Validate(model.Address);
            if (validator.IsValid)
            {
                var user = _unitOfWork.GetRepo<Customer>().GetObject(x => x.Email == User.Identity.Name);
                model.Address.CustomerId = user.Id;
                model.Address.CreatedOnUtc = DateTime.Now;
                _unitOfWork.GetRepo<Address>().Add(model.Address);
            }

            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError("Address."+a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Adres ekleme işlemi başarılı bir şekilde gerçekleştirildi." : "Adres ekleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("MyAccount");
        }

        public ActionResult AddressDelete(int id)
        {
            _unitOfWork.GetRepo<Address>().Delete(id);
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Adres silme işlemi başarılı bir şekilde gerçekleştirildi." : "Adres silme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("MyAccount");
        }
        [CustomAuthorization(Roles = "Üye,Admin")]
        public ActionResult WishList()
        {
            var user = _unitOfWork.GetRepo<Customer>().GetObject(x => x.Email == User.Identity.Name);
            var model = _unitOfWork.GetRepo<Wishlist>().Where(x => x.CustomerId == user.Id).ToList();
            return View(model);
        }
        [CustomAuthorization(Roles = "Üye,Admin")]
        public ActionResult WishListAdd(int id)
        {
            var model = new Wishlist
            {
                ProductId = id,
                CustomerId = _unitOfWork.GetRepo<Customer>().GetObject(x => x.Email == User.Identity.Name).Id
            };
            _unitOfWork.GetRepo<Wishlist>().Add(model);
            _unitOfWork.Commit();
            return RedirectToAction("WishList");
        }
        [CustomAuthorization(Roles = "Üye,Admin")]
        public ActionResult WishListDelete(int id)
        {
            _unitOfWork.GetRepo<Wishlist>().Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("WishList");
        }
        #region Loginİşlemleri
        public ActionResult SignIn()
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SignIn(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.LoginModel.Password = _encryptor.Hash(model.LoginModel.Password);
                var customer = _unitOfWork.GetRepo<Customer>().Where(x => x.Email == model.LoginModel.Email && x.Password == model.LoginModel.Password).FirstOrDefault();

                if (customer != null)
                {

                    FormsAuthentication.SetAuthCookie(customer.Email, model.LoginModel.RememberMe);
                    customer.LastLoginTime = DateTime.Now;
                    _unitOfWork.Commit();
                    return Redirect("/");
                }
            }
            TempData["IsSuccess"] = false;
            TempData["ModelState"] = ModelState;
            TempData["Message"] = "Kullanıcı Adı veya Şifre Hatalı";
            return RedirectToAction("SignIn");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");

        }
        #endregion

        #region Kayıtİşlemi
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SignUp(AccountViewModel model)
        {
            var validator = new SiteCustomerRegisterValidator(_unitOfWork, model.PasswordConfirm).Validate(model.Customer);
            if (validator.IsValid)
            {
                model.Customer.CreatedTime = DateTime.Now;
                model.Customer.CustomerRoleId = 2;
                model.Customer.Active = true;
                model.Customer.Password = _encryptor.Hash(model.Customer.Password);
                _unitOfWork.GetRepo<Customer>().Add(model.Customer);
                var defaultPicture = _customerPictureService.GetDefaultImage();
                var picture = _unitOfWork.GetRepo<Picture>()
                    .Where(x => x.PicturePath == defaultPicture).FirstOrDefault();
                _unitOfWork.GetRepo<CustomerPicture>().Add(new CustomerPicture
                {
                    Customer = model.Customer,
                    Picture = picture
                });
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError(a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Hesap oluşturulma işlemi başarılı bir şekilde gerçekleştirildi.Aşağıdan giriş yapabilirsiniz." : "Hesap oluşturma işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("SignIn");
        }
        #endregion

        #region SifreSifirlamaİslemleri
        public JsonResult ForgetPassword(string email)
        {
            var model = _unitOfWork.GetRepo<Customer>().GetObject(x => x.Email == email);
            var modelEmail = _unitOfWork.GetRepo<EmailAccount>().GetAll().FirstOrDefault();
            var result = new ForgetPasswordResult();
            if (model != null)
            {
                var token = Guid.NewGuid().ToString();
                var url = "http://" + Request.Url.Authority + Url.Action("PasswordReset", "Account", new { token });
                model.PasswordResetToken = token;
                var msg = new MessageTemplate()
                {
                    From = modelEmail.Email,
                    MessageBody = MailTemplateReader(model.Name + " " + model.Surname, url),
                    MessageSubject = "Parola Sıfırlama",
                    To = email
                };
                _messaging.SendMessage(msg);
                if (_messaging.IsSucceed)
                {
                    _unitOfWork.GetRepo<Customer>().Update(model);
                    _unitOfWork.Commit();
                    result.Message = "Şifre sıfırlama linki mail adresinize gönderilmiştir.";
                    result.AlertType = "success";
                }
                else
                {
                    result.Message = "Şifre sıfırlama linki mail gönderilirken bir hata oluştu.";
                    result.AlertType = "danger";
                }
            }
            else
            {
                result.Message = "Girdiğiniz mail adresi sistemimizde bulunamamıştır.Lütfen tekrar deneyiniz.";
                result.AlertType = "danger";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PasswordReset(string token)
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
            var user = _unitOfWork.GetRepo<Customer>().GetObject(x => x.PasswordResetToken == token);
            if (user != null)
            {
                return View(new AccountViewModel { Customer = user });
            }
            TempData["IsSuccess"] = false;
            TempData["ModelState"] = ModelState;
            TempData["Message"] = "Bu şifre sıfırlama bağlantısı artık mevcut değil.";
            return RedirectToAction("SignIn");
        }
        [ValidateAntiForgeryToken, HttpPost]
        public ActionResult PasswordReset(AccountViewModel model)
        {
            var validator = new PasswordResetValidator(model.PasswordConfirm).Validate(model.Customer);
            if (validator.IsValid)
            {
                var user = _unitOfWork.GetRepo<Customer>().GetObject(x => x.PasswordResetToken == model.Customer.PasswordResetToken);
                user.Password = model.Customer.Password;
                user.PasswordResetToken = null;
                _unitOfWork.GetRepo<Customer>().Update(user);
            }
            var sonuc = _unitOfWork.Commit();
            if (sonuc)
            {
                TempData["IsSuccess"] = sonuc;
                TempData["ModelState"] = ModelState;
                TempData["Message"] = "Şifre sıfırlama işlemi başarıyla gerçekleştirildi. Aşağıdan giriş yapabilirsiniz.";
                return RedirectToAction("SignIn");
            }
            TempData["IsSuccess"] = sonuc;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError("Customer." + a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = "Şifre sıfırlama işlemi gerçekleştirilemedi. Lütfen tekrar deneyiniz.";
            return RedirectToAction("PasswordReset", new { token = model.Customer.PasswordResetToken });
        }

        private string MailTemplateReader(string mailTo, string url)
        {
            var modelSite = _unitOfWork.GetRepo<Setting>().GetAll().FirstOrDefault();
            string body;
            using (var sr = new StreamReader(Server.MapPath("/App_Data/Templates/") + "MailTemplate.html"))
            {
                body = sr.ReadToEnd();
            }
            body = body.Replace("{{company_name}}", modelSite.CompanyName);
            body = body.Replace("{{name}}", mailTo);
            body = body.Replace("{{action_url}}", url);
            body = body.Replace("{{city}}", modelSite.City);
            body = body.Replace("{{town}}", modelSite.Town);

            return body;
        }

        #endregion
    }
}