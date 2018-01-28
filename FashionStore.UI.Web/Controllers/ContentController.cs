using System;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using System.Linq;
using System.Web.Mvc;
using FashionStore_BLL.Services.Concretes;
using FashionStore_BLL.Validations.ContactUsValidations;

namespace FashionStore.UI.Web.Controllers
{
    public class ContentController : BaseController
    {
        public ContentController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        // GET: Content
        public ActionResult GetPage(string SeoUrl)
        {
            var model = _unitOfWork.GetRepo<ContentPage>().GetObject(x => x.SeoUrl == SeoUrl);
            ViewBag.Title = model.Title;
            return View(model);
        }
        public ActionResult SSS()
        {
            var model = _unitOfWork.GetRepo<FrequantlyQuestion>().GetAll().ToList();

            var modelContact = _unitOfWork.GetRepo<Setting>().GetAll().FirstOrDefault();
            var modelEmail = _unitOfWork.GetRepo<EmailAccount>().GetAll().FirstOrDefault();
            ViewBag.Phone = modelContact?.PhoneNumber;
            ViewBag.Email = modelEmail?.Email;
            ViewBag.Address = modelContact?.City + " / " + modelContact?.Town;
            return View(model);
        }
        public ActionResult ContactUs()
        {
            ViewBag.Mail = _unitOfWork.GetRepo<EmailAccount>().GetAll().FirstOrDefault()?.Email;
            var model = _unitOfWork.GetRepo<Setting>().GetAll().FirstOrDefault();
            return View(model);
        }

        public JsonResult WriteUs(string name, string mail, string subject, string comment)
        {
            var result = new ForgetPasswordResult();
            var model = new Message()
            {
                Name = name,
                Mail = mail,
                Subject = subject,
                Comment = comment,
                CreatedTime = DateTime.Now,
                IsReaded = false
                
            };
            var validator = new ContactUsValidator().Validate(model);
            if (validator.IsValid)
            {
                _unitOfWork.GetRepo<Message>().Add(model);
                _unitOfWork.Commit();
                result.Message = "Mesajınız başarılı bir şekilde iletildi. En kısa zamanda tarafınıza dönüş yapılacaktır.";
                result.AlertType = "success";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result.Message = "Ad,Mail,Konu ve Mesaj alanları boş bırakılamaz.Lütfen tekrar deneyiniz.";
            result.AlertType = "danger";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}