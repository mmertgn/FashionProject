using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Controllers;
using FashionStore_BLL.Validations.SettingValidations;
using System.Linq;
using System.Web.Mvc;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class SettingsController : BaseController
    {
        public SettingsController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

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
                    settingModel.AboutUs = model.AboutUs;
                    settingModel.ConfidentialityAgreement = model.ConfidentialityAgreement;
                    settingModel.TermsAgreement = model.TermsAgreement;
                    settingModel.SalesContract = model.SalesContract;
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