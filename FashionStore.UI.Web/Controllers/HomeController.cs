using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace FashionStore.UI.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        // GET: Home
        public ActionResult Index()
        {
            var model = new HomePageViewModel
            {
                SliderPictures = _unitOfWork.GetRepo<SliderPicture>().GetAll().OrderBy(x => x.Slider.DisplayOrder).ToList(),
                NewArrivalProducts = _unitOfWork.GetRepo<Product>().Where(x => x.MarkAsNew && x.MarkAsNewStartTime <= DateTime.Now && x.MarkAsNewEndTime >= DateTime.Now && x.Active && !x.Deleted).Take(12).OrderBy(x => x.DisplayOrder).ToList(),
                BestSellerProducts = _unitOfWork.GetRepo<Product>().Where(x => x.IsFeaturedProduct && x.Active && !x.Deleted).Take(12).OrderBy(x => x.DisplayOrder).ToList()
            };
            return View(model);
        }

        public PartialViewResult GetCategories()
        {
            var model = _unitOfWork.GetRepo<Category>().Where(x => x.ParentCategoryId == null && !x.Deleted && x.ShowOnTopMenu).OrderBy(x => x.DisplayOrder).ToList();
            return PartialView("_PartialCategory", model);
        }

        public PartialViewResult HeaderPages()
        {
            var model = _unitOfWork.GetRepo<ContentPage>().Where(x => x.ShowOnHeader).OrderBy(x => x.DisplayOrder).ToList();
            return PartialView("_PartialHeaderPages", model);
        }


        public PartialViewResult SocialMediaLinks()
        {
            var model = _unitOfWork.GetRepo<Setting>().GetAll().FirstOrDefault();
            return PartialView("_PartialSocialMedia", model);
        }
        public PartialViewResult FooterAboutUsAddressSocialMedia()
        {
            ViewBag.Mail = _unitOfWork.GetRepo<EmailAccount>().GetAll().FirstOrDefault()?.Email;
            var model = _unitOfWork.GetRepo<Setting>().GetAll().FirstOrDefault();
            return PartialView("_PartialFooterAboutUsAddressSocialMedia", model);
        }
        public PartialViewResult FooterColumns()
        {
            var model = _unitOfWork.GetRepo<ContentPage>().GetAll().OrderBy(x => x.DisplayOrder);
            return PartialView("_PartialFooterColumns", model);
        }
        public PartialViewResult HeaderLoginPanel()
        {
            var model = _unitOfWork.GetRepo<Customer>().GetObject(x => x.Email == HttpContext.User.Identity.Name);
            if (model != null)
            {
                ViewBag.Name = model.Name + " " + model.Surname;
                ViewBag.Role = model.CustomerRole.Name;
            }
            return PartialView("_PartialHeaderLoginPanel");
        }
    }
}