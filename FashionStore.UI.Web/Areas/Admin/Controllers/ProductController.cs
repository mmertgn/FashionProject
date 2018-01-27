using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Areas.Admin.Models;
using FashionStore.UI.Web.Controllers;
using FashionStore_BLL.Services.Abstracts;
using FashionStore_BLL.Validations.ProductValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity.Attributes;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class ProductController : BaseController
    {
        private readonly ISeoUrlMaker _seoUrlMaker;
        private readonly IUploadService _uploadService;
        public ProductController(
            IUnitOfWork unitOfWork,
            ISeoUrlMaker seoUrlMaker,
            [Dependency("PhotoUpload")]IUploadService uploadService) : base(unitOfWork)
        {
            _seoUrlMaker = seoUrlMaker;
            _uploadService = uploadService;
        }
        public ActionResult List()
        {
            var products = _unitOfWork.GetRepo<Product>().Where(x => x.Deleted == false).ToList();
            return View(products);
        }
        public ActionResult Detail(string SeoUrl)
        {
            var productPictureModel =
                _unitOfWork.GetRepo<ProductPicture>().Where(x => x.Product.SeoUrl == SeoUrl).ToList();
            var product = productPictureModel.FirstOrDefault();
            if (product != null)
            {
                var model = new ProductViewModel
                {
                    ProductPictures = productPictureModel,
                    Product = product.Product
                };
                return View(model);
            }

            return RedirectToAction("List");
        }
        public ActionResult Add()
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);

            var categories = _unitOfWork.GetRepo<Category>().Where(x => x.ParentCategoryId == null);
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            var model = new ProductViewModel
            {
                Product = new Product()
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Add(ProductViewModel model)
        {
            var validator = new ProductAddValidator(_unitOfWork).Validate(model.Product);
            if (validator.IsValid)
            {
                model.Product.CategoryId = model.Product.CategoryId != 0 ? model.Product.CategoryId : model.CatId;
                model.Product.SeoUrl = _seoUrlMaker.MakeSlug(model.Product.Name);
                model.Product.CreateTime = DateTime.Now;
                _unitOfWork.GetRepo<Product>().Add(model.Product);

                foreach (var picture in model.PostedPictures)
                {
                    var pictureModel = new Picture();
                    var picturePath = _uploadService.Upload(picture);
                    pictureModel.SeoFileName = _seoUrlMaker.MakeSlug(picture.FileName);
                    pictureModel.AltAttribute = _seoUrlMaker.MakeSlug(picture.FileName);
                    pictureModel.TitleAttribute = _seoUrlMaker.MakeSlug(picture.FileName);
                    pictureModel.PicturePath = picturePath;
                    _unitOfWork.GetRepo<Picture>().Add(pictureModel);
                    _unitOfWork.GetRepo<ProductPicture>().Add(new ProductPicture
                    {
                        Product = model.Product,
                        Picture = pictureModel
                    });
                }
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError("Product." + a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Ürün ekleme işlemi başarılı bir şekilde gerçekleştirildi." : "Ürün ekleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";

            return RedirectToAction("Add");
        }
        public ActionResult Edit(string SeoUrl)
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);

            var productPictureModel = _unitOfWork.GetRepo<ProductPicture>().Where(x => x.Product.SeoUrl == SeoUrl).ToList();
            var productModel = productPictureModel.FirstOrDefault();

            var categories = _unitOfWork.GetRepo<Category>().Where(x => x.ParentCategoryId == null);
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            var childCategories = _unitOfWork.GetRepo<Category>().Where(x => x.ParentCategoryId == productModel.Product.Category.ParentCategoryId);
            ViewBag.ChildCategories = new SelectList(childCategories, "Id", "Name");

            if (productModel != null)
            {
                var model = new ProductViewModel()
                {
                    Product = productModel.Product,
                    ProductPictures = productPictureModel
                };
                return View(model);
            }

            return RedirectToAction("List");
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit(ProductViewModel model)
        {
            var validator = new ProductUpdateValidator(_unitOfWork).Validate(model.Product);
            if (validator.IsValid)
            {
                model.Product.UpdateTime = DateTime.Now;
                model.Product.Category = null;
                _unitOfWork.GetRepo<Product>().Update(model.Product);
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError("Product." + a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Ürün bilgileri güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Ürün bilgileri güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("Edit", new { model.Product.SeoUrl });
        }
        public ActionResult Delete(string SeoUrl)
        {
            var modelProduct = _unitOfWork.GetRepo<Product>().Where(x => x.SeoUrl == SeoUrl).FirstOrDefault();
            if (modelProduct != null)
            {
                modelProduct.Deleted = true;
                _unitOfWork.GetRepo<Product>().Update(modelProduct);
            }

            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            TempData["Message"] = isSuccess ? "Ürün silme işlemi başarılı bir şekilde gerçekleştirildi." : "Ürün silme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("List");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ProductImageUpload(IEnumerable<HttpPostedFileBase> PostedPictures, string SeoUrl)
        {
            var httpPostedFileBases = PostedPictures.ToList();
            if (httpPostedFileBases.FirstOrDefault() != null)
            {
                var productModel = _unitOfWork.GetRepo<Product>().Where(x => x.SeoUrl == SeoUrl).FirstOrDefault();
                foreach (var picture in httpPostedFileBases)
                {
                    var pictureModel = new Picture();
                    var picturePath = _uploadService.Upload(picture);
                    pictureModel.SeoFileName = _seoUrlMaker.MakeSlug(picture.FileName);
                    pictureModel.AltAttribute = _seoUrlMaker.MakeSlug(picture.FileName);
                    pictureModel.TitleAttribute = _seoUrlMaker.MakeSlug(picture.FileName);
                    pictureModel.PicturePath = picturePath;
                    _unitOfWork.GetRepo<Picture>().Add(pictureModel);
                    _unitOfWork.GetRepo<ProductPicture>().Add(new ProductPicture
                    {
                        Product = productModel,
                        Picture = pictureModel
                    });
                }
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Ürün Resmi ekleme işlemi başarılı bir şekilde gerçekleştirildi." : "Ürün Resmi ekleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";

            return RedirectToAction("Edit", new { SeoUrl });
        }
        public JsonResult ProductPhotoDelete(string picturePath)
        {
            var model = _unitOfWork.GetRepo<ProductPicture>().Where(x => x.Picture.PicturePath == picturePath).FirstOrDefault();
            if (model != null)
            {
                var fullPath = Server.MapPath("~/Uploads/" + model.Picture.PicturePath);

                if (!System.IO.File.Exists(fullPath)) return Json(model.Picture.PicturePath, JsonRequestBehavior.AllowGet);

                System.IO.File.Delete(fullPath);
                _unitOfWork.GetRepo<Picture>().Delete(model.Picture.Id);
            }
            _unitOfWork.Commit();
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCategories(int? ParentCatId)
        {
            if (ParentCatId == null) return Json("", JsonRequestBehavior.DenyGet);
            var catList = _unitOfWork.GetRepo<Category>().Where(x => x.ParentCategoryId == ParentCatId).Select(x => new CatModel { Id = x.Id, Name = x.Name }).ToList();
            return Json(catList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ReviewList()
        {
            var reviews = _unitOfWork.GetRepo<ProductReview>().GetAll().ToList();
            return View(reviews);
        }
        public ActionResult ReviewDetail(int SeoUrl)
        {
            var review = _unitOfWork.GetRepo<ProductReview>().GetObject(x => x.Id == SeoUrl);
            return View(review);
        }
        public ActionResult ReviewEdit(int SeoUrl)
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
            var review = _unitOfWork.GetRepo<ProductReview>().GetObject(x => x.Id == SeoUrl);

            return View(review);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult ReviewEdit(ProductReview model)
        {
            var validator = new ProductReviewUpdateValidator().Validate(model);
            if (validator.IsValid)
            {
                var reviewModel = _unitOfWork.GetRepo<ProductReview>().GetObject(x => x.Id == model.Id);
                reviewModel.Title = model.Title;
                reviewModel.ReviewText = model.ReviewText;
                reviewModel.Rating = model.Rating;
                reviewModel.HelpfulYesTotal = model.HelpfulYesTotal;
                reviewModel.HelpfulNoTotal = model.HelpfulNoTotal;
                reviewModel.IsApproved = model.IsApproved;
                _unitOfWork.GetRepo<ProductReview>().Update(reviewModel);
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError(a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Yorum güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Yorum güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("ReviewEdit", new { model.Id });
        }

        private class CatModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}