using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using System.Linq;
using System.Web.Mvc;

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
    }
}