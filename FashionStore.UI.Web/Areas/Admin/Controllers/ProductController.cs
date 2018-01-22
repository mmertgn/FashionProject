using System;
using System.Collections.Generic;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Controllers;
using FashionStore_BLL.Services.Abstracts;
using System.Linq;
using System.Web.Mvc;
using FashionStore.UI.Web.Areas.Admin.Models;
using FashionStore_BLL.Validations.ProductValidations;
using Unity.Attributes;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ISeoUrlMaker _seoUrlMaker;
        private readonly IUploadService _uploadService;
        private readonly IPictureService _productPictureService;
        public ProductController(
            IUnitOfWork unitOfWork,
            ISeoUrlMaker seoUrlMaker,
            [Dependency("ProductPicture")]IPictureService productPictureService,
            [Dependency("PhotoUpload")]IUploadService uploadService) : base(unitOfWork)
        {
            _seoUrlMaker = seoUrlMaker;
            _uploadService = uploadService;
            _productPictureService = productPictureService;
        }
        public ActionResult List()
        {
            var products = _unitOfWork.GetRepo<Product>().GetAll().ToList();
            return View(products);
        }
        public ActionResult Detail(string SeoUrl)
        {
            var productPictureModel =
                _unitOfWork.GetRepo<ProductPicture>().Where(x => x.Product.SeoUrl == SeoUrl).ToList();
            var product = productPictureModel.FirstOrDefault();
            var model = new ProductViewModel
            {
                ProductPictures = productPictureModel,
                Product = product.Product
            };
            return View(model);
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

        public ActionResult Edit()
        {
            return View();
        }

        public JsonResult GetCategories(int? ParentCatId)
        {
            if (ParentCatId == null) return Json("", JsonRequestBehavior.DenyGet);
            var catList = _unitOfWork.GetRepo<Category>().Where(x => x.ParentCategoryId == ParentCatId).Select(x => new CatModel() { Id = x.Id, Name = x.Name }).ToList();
            return Json(catList, JsonRequestBehavior.AllowGet);

        }

        private class CatModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}