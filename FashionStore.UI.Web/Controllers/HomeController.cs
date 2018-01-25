using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Models;

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
                SliderPictures = _unitOfWork.GetRepo<SliderPicture>().GetAll().OrderBy(x=>x.Slider.DisplayOrder).ToList(),
                NewArrivalsProducts = _unitOfWork.GetRepo<ProductPicture>().Where(x => x.Product.MarkAsNew).ToList()
            };
            return View(model);
        }

        public PartialViewResult GetCategories()
        {
            var model = _unitOfWork.GetRepo<Category>().Where(x => x.ParentCategoryId == null).OrderBy(x=>x.DisplayOrder).ToList();
            return PartialView("_PartialCategory",model);
        }

        public PartialViewResult HeaderPages()
        {
            var model = _unitOfWork.GetRepo<ContentPage>().Where(x => x.ShowOnHeader).ToList();
            return PartialView("_PartialHeaderPages",model);
        }


        public PartialViewResult SocialMediaLinks()
        {
            var model = _unitOfWork.GetRepo<Setting>().GetAll().FirstOrDefault();
            return PartialView("_PartialSocialMedia",model);
        }
        public PartialViewResult FooterAboutUsAddressSocialMedia()
        {
            ViewBag.Mail = _unitOfWork.GetRepo<EmailAccount>().GetAll().FirstOrDefault().Email;
            var model = _unitOfWork.GetRepo<Setting>().GetAll().FirstOrDefault();
            return PartialView("_PartialFooterAboutUsAddressSocialMedia", model);
        }
        public PartialViewResult FooterColumns()
        {
            var model = _unitOfWork.GetRepo<ContentPage>().GetAll();
            return PartialView("_PartialFooterColumns", model);
        }
    }
}