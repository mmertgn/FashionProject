using System.IO;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Controllers;
using FashionStore_BLL.Validations.SettingValidations;
using System.Linq;
using System.Web.Mvc;
using FashionStore_BLL.Services.Abstracts;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorization(Roles = "Admin")]

    public class SettingsController : BaseController
    {
        private readonly IMessaging _messaging;
        public SettingsController(IUnitOfWork unitOfWork,
            IMessaging messaging) : base(unitOfWork)
        {
            _messaging = messaging;
        }

        #region MailBox

        public ActionResult List()
        {
            var model = _unitOfWork.GetRepo<Message>().GetAll().OrderBy(x => x.CreatedTime);
            return View(model);
        }
        public ActionResult MessageDetail(int id)
        {
            var model = _unitOfWork.GetRepo<Message>().GetObject(x => x.Id == id);
            model.IsReaded = true;
            _unitOfWork.GetRepo<Message>().Update(model);
            _unitOfWork.Commit();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SendAnswer(Message model)
        {
            if (model.AdminAnswer != null)
            {
                var modelEmail = _unitOfWork.GetRepo<EmailAccount>().GetAll().FirstOrDefault();
                var msg = new MessageTemplate()
                {
                    From = modelEmail.Email,
                    MessageBody = MailTemplateReader(model.Name, model.AdminAnswer),
                    MessageSubject = "Bizimle İletişime Geçtiğiniz için Teşekkürler!",
                    To = model.Mail
                };
                _messaging.SendMessage(msg);
                if (_messaging.IsSucceed)
                {
                    _unitOfWork.GetRepo<Message>().Update(model);
                }
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Mesajınız kullanıcıya başarılı bir şekilde gönderildi." : "Mesaj gönderme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("MessageDetail", "Settings", new { model.Id });
        }
        private string MailTemplateReader(string name, string message)
        {
            var modelSite = _unitOfWork.GetRepo<Setting>().GetAll().FirstOrDefault();
            string body;
            using (var sr = new StreamReader(Server.MapPath("/App_Data/Templates/") + "MailAnswer.html"))
            {
                body = sr.ReadToEnd();
            }
            body = body.Replace("{{company_name}}", modelSite.CompanyName);
            body = body.Replace("{{name}}", name);
            body = body.Replace("{{message}}", message);
            body = body.Replace("{{city}}", modelSite.City);
            body = body.Replace("{{town}}", modelSite.Town);

            return body;
        }
        #endregion
        #region SosyalMedyaİşlemleri
        public ActionResult SocialMediaSettings()
        {
            var settingModel = _unitOfWork.GetRepo<Setting>().GetAll().FirstOrDefault();
            return View(settingModel ?? new Setting());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SocialMediaSettings(Setting model)
        {
            var validator = new SocialMediaSettingsValidator().Validate(model);
            if (validator.IsValid)
            {
                var settingModel = _unitOfWork.GetRepo<Setting>().GetAll().FirstOrDefault();
                if (settingModel != null)
                {
                    settingModel.FacebookUrl = model.FacebookUrl;
                    settingModel.GoogleUrl = model.GoogleUrl;
                    settingModel.InstagramUrl = model.InstagramUrl;
                    settingModel.PinterestUrl = model.PinterestUrl;
                    settingModel.TwitterUrl = model.TwitterUrl;
                    _unitOfWork.GetRepo<Setting>().Update(settingModel);
                }
                else
                {
                    _unitOfWork.GetRepo<Setting>().Add(model);
                }
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError(a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Sosyal medya ayarları güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Sosyal medya ayarları güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";

            return RedirectToAction("SocialMediaSettings");
        }
        #endregion
        #region GenelAyarlar
        public ActionResult GeneralSettingsEdit()
        {
            var settingModel = _unitOfWork.GetRepo<Setting>().GetAll().FirstOrDefault();
            return View(settingModel ?? new Setting());
        }
        [HttpPost, ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult GeneralSettingsEdit(Setting model)
        {
            var validator = new GeneralSettingsValidator().Validate(model);
            if (validator.IsValid)
            {
                var settingModel = _unitOfWork.GetRepo<Setting>().GetAll().FirstOrDefault();
                if (settingModel != null)
                {
                    settingModel.Id = model.Id;
                    settingModel.CompanyName = model.CompanyName;
                    settingModel.MetaTitle = model.MetaTitle;
                    settingModel.MetaDescription = model.MetaDescription;
                    settingModel.Address = model.Address;
                    settingModel.Town = model.Town;
                    settingModel.City = model.City;
                    settingModel.PhoneNumber = model.PhoneNumber;
                    settingModel.FaxNumber = model.FaxNumber;
                    settingModel.MapXCoordinate = model.MapXCoordinate;
                    settingModel.MapYCoordinate = model.MapYCoordinate;
                    _unitOfWork.GetRepo<Setting>().Update(settingModel);
                }
                else
                {
                    _unitOfWork.GetRepo<Setting>().Add(model);
                }
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError(a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Site ayarları güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Site ayarları güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";

            return RedirectToAction("GeneralSettingsEdit");
        }
        #endregion
        #region Emailİşlemleri
        public ActionResult EmailEdit()
        {
            var emailModel = _unitOfWork.GetRepo<EmailAccount>().GetAll().FirstOrDefault();
            return View(emailModel ?? new EmailAccount());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EmailEdit(EmailAccount model)
        {
            var validator = new EmailAccountValidator().Validate(model);
            if (validator.IsValid)
            {
                var emailModel = _unitOfWork.GetRepo<EmailAccount>().GetAll().FirstOrDefault();
                if (emailModel != null)
                {
                    emailModel.Id = model.Id;
                    emailModel.Email = model.Email;
                    emailModel.EnableSsl = model.EnableSsl;
                    emailModel.Host = model.Host;
                    emailModel.Password = model.Password;
                    emailModel.Port = model.Port;
                    emailModel.UseDefaultCredentials = model.UseDefaultCredentials;
                    _unitOfWork.GetRepo<EmailAccount>().Update(emailModel);
                }
                else
                {
                    _unitOfWork.GetRepo<EmailAccount>().Add(model);
                }
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError(a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Email güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Email güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";

            return RedirectToAction("EmailEdit");
        }
        #endregion
        #region Menu İşlemleri
        public ActionResult MenuList()
        {
            var model = _unitOfWork.GetRepo<AdminMenuBar>().GetAll().OrderBy(x => x.ParentSidebarId).ToList();
            return View(model);
        }
        public ActionResult MenuAdd()
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);

            var categories = _unitOfWork.GetRepo<AdminMenuBar>().GetAll();
            ViewBag.Menus = new SelectList(categories, "Id", "Name");
            return View(new AdminMenuBar());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult MenuAdd(AdminMenuBar model)
        {
            var validator = new AdminMenuAddValidator().Validate(model);
            if (validator.IsValid)
            {
                _unitOfWork.GetRepo<AdminMenuBar>().Add(model);
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError(a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Menu ekleme işlemi başarılı bir şekilde gerçekleştirildi." : "Menu ekleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";

            return RedirectToAction("MenuAdd");
        }
        public ActionResult MenuEdit(int id)
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);

            var categories = _unitOfWork.GetRepo<AdminMenuBar>().GetAll();
            ViewBag.Menus = new SelectList(categories, "Id", "Name");
            var menuModel = _unitOfWork.GetRepo<AdminMenuBar>().GetObject(x => x.Id == id);

            return View(menuModel);
        }
        [HttpPost]
        public ActionResult MenuEdit(AdminMenuBar menuModel)
        {
            var validator = new AdminMenuUpdateValidator().Validate(menuModel);
            if (validator.IsValid)
            {
                _unitOfWork.GetRepo<AdminMenuBar>().Update(menuModel);
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError("Category." + a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Menü bilgileri güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Menü bilgileri güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("MenuEdit", new { id = menuModel.Id });
        }

        public ActionResult MenuDelete(int id)
        {
            _unitOfWork.GetRepo<AdminMenuBar>().Delete(id);
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            TempData["Message"] = isSuccess ? "Menü silme işlemi başarılı bir şekilde gerçekleştirildi." : "Menü silme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("MenuList");
        }
        #endregion
    }
}