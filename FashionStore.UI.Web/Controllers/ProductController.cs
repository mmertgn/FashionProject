using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Models;
using FashionStore_BLL.Services.Concretes;

namespace FashionStore.UI.Web.Controllers
{
    public class ProductController : BaseController
    {

        public ActionResult List(string SeoUrl)
        {
            
            return View();
        }

        // GET: Product
        public ActionResult Detail(string SeoUrl)
        {
            var product = _unitOfWork.GetRepo<Product>().GetObject(x => x.SeoUrl == SeoUrl);
            var model = new ProductDetailViewModel
            {
                Product = product,
                RelatedProducts = _unitOfWork.GetRepo<Product>().Where(x => x.CategoryId == product.CategoryId).ToList()
            };
            return View(model);
        }

        public JsonResult ReviewAdd(string title, string review, string SeoUrl)
        {
            var result = new ForgetPasswordResult();
            if (User.Identity.IsAuthenticated)
            {
                if (title == null && review == null)
                {
                    result.Message = "Yorum başlığı ve yorum içeriği alanları boş geçilemez.";
                    result.AlertType = "danger";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var product = _unitOfWork.GetRepo<Product>().GetObject(x => x.SeoUrl == SeoUrl);
                var user = _unitOfWork.GetRepo<Customer>().GetObject(x => x.Email == User.Identity.Name);
                var reviewModel = new ProductReview
                {
                    ProductId = product.Id,
                    CustomerId = user.Id,
                    IsApproved = true,
                    Title = title,
                    ReviewText = review,
                    CreatedTime = DateTime.Now,
                    Rating = 0,
                    HelpfulYesTotal = 0,
                    HelpfulNoTotal = 0
                };
                _unitOfWork.GetRepo<ProductReview>().Add(reviewModel);
                _unitOfWork.Commit();
                result.Message = "Yorumunuz başarıyla kaydedilmiştir.";
                result.AlertType = "success";
                return Json(result, JsonRequestBehavior.AllowGet);

            }

            result.Message = "Ürüne yorum bırakabilmek için giriş yapmanız gerekmektedir.Lütfen kullanıcı girişi yapınız.";
            result.AlertType = "danger";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ProductController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}