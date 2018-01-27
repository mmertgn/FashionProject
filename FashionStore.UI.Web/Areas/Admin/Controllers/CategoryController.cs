using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Controllers;
using FashionStore_BLL.Services.Abstracts;
using FashionStore_BLL.Validations.CategoryValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity.Attributes;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class CategoryController : BaseController
    {
        private readonly ISeoUrlMaker _seoUrlMaker;
        private readonly IPictureService _categoryPictureService;
        private readonly IUploadService _uploadService;
        public CategoryController(IUnitOfWork unitOfWork,
            ISeoUrlMaker seoUrlMaker,
            [Dependency("CategoryPicture")]IPictureService categoryPictureService,
            [Dependency("PhotoUpload")]IUploadService uploadService) : base(unitOfWork)
        {
            _categoryPictureService = categoryPictureService;
            _seoUrlMaker = seoUrlMaker;
            _uploadService = uploadService;
        }

        public ActionResult List()
        {

            var model = _unitOfWork.GetRepo<Category>().GetAll().Where(x => x.Deleted == false);
            return View(model);
        }
        private class IsActiveModel
        {
            public string Name { get; set; }
            public bool Deger { get; set; }
        }
        List<IsActiveModel> isActiveList = new List<IsActiveModel>
        {
            new IsActiveModel{Name = "Aktif",Deger = true},
            new IsActiveModel{Name = "Pasif",Deger = false}
        };
        public ActionResult Add()
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);

            var categories = _unitOfWork.GetRepo<Category>().Where(x => x.ParentCategoryId == null);
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            ViewBag.isActive = new SelectList(isActiveList, "Deger", "Name");
            return View(new Category());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            var validator = new CategoryAddValidator(_unitOfWork).Validate(model);
            if (validator.IsValid)
            {
                var parentCat = _unitOfWork.GetRepo<Category>().GetObject(x => x.Id == model.ParentCategoryId);
                model.SeoUrl = model.ParentCategoryId == null ? _seoUrlMaker.MakeSlug(model.Name) : _seoUrlMaker.MakeSlug(parentCat.Name + "-" + model.Name);
                model.CreateTime = DateTime.Now;
                _unitOfWork.GetRepo<Category>().Add(model);
                var defaultPicture = _categoryPictureService.GetDefaultImage();
                var picture = _unitOfWork.GetRepo<Picture>()
                    .Where(x => x.PicturePath == defaultPicture).FirstOrDefault();
                _unitOfWork.GetRepo<CategoryPicture>().Add(new CategoryPicture
                {
                    Category = model,
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
            TempData["Message"] = isSuccess ? "Kategori ekleme işlemi başarılı bir şekilde gerçekleştirildi." : "Kategori ekleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";

            return RedirectToAction("Add");
        }

        public ActionResult Detail(string SeoUrl)
        {
            var categoryModel = _unitOfWork.GetRepo<CategoryPicture>().GetObject(x => x.Category.SeoUrl == SeoUrl);
            return View(categoryModel);
        }
        public ActionResult Edit(string SeoUrl)
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);

            var categoryModel = _unitOfWork.GetRepo<CategoryPicture>().GetObject(x => x.Category.SeoUrl == SeoUrl);

            var categories = _unitOfWork.GetRepo<Category>().GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.isActive = new SelectList(isActiveList, "Deger", "Name");

            return View(categoryModel);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryPicture model)
        {
            var validator = new CategoryUpdateValidator(_unitOfWork).Validate(model.Category);
            if (validator.IsValid)
            {
                model.Category.UpdateTime = DateTime.Now;
                _unitOfWork.GetRepo<Category>().Update(model.Category);
            }

            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError("Category." + a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Kategori bilgileri güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Kategori bilgileri güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("Edit", new { SeoUrl = model.Category.SeoUrl });
        }

        public ActionResult Delete(string SeoUrl)
        {
            var modelCategory = _unitOfWork.GetRepo<Category>().Where(x => x.SeoUrl == SeoUrl).FirstOrDefault();
            modelCategory.Deleted = true;
            _unitOfWork.GetRepo<Category>().Update(modelCategory);
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            TempData["Message"] = isSuccess ? "Kategori silme işlemi başarılı bir şekilde gerçekleştirildi." : "Kategori silme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("List");
        }

        [HttpPost]
        public JsonResult CategoryPhotoUpload(HttpPostedFileBase categoryphoto, string SeoUrl)
        {
            if (categoryphoto == null) return Json("");

            var model = _unitOfWork.GetRepo<CategoryPicture>().GetObject(x => x.Category.SeoUrl == SeoUrl);
            var picturePath = _uploadService.Upload(categoryphoto);
            model.Picture.SeoFileName = _seoUrlMaker.MakeSlug(categoryphoto.FileName);
            model.Picture.AltAttribute = _seoUrlMaker.MakeSlug(categoryphoto.FileName);
            model.Picture.TitleAttribute = categoryphoto.FileName;
            model.Picture.PicturePath = picturePath;
            _unitOfWork.GetRepo<CategoryPicture>().Update(model);

            var isSuccess = _unitOfWork.Commit();
            ViewBag.IsSuccess = isSuccess;
            ViewBag.Message = isSuccess ? "Kategori resmi başarılı bir şekilde güncelleştirildi." : "Kategori resmi güncelleştirilemedi, lütfen tekrar deneyiniz.";
            return Json(picturePath);
        }
        public JsonResult CategoryPhotoDelete(string SeoUrl)
        {
            var model = _unitOfWork.GetRepo<CategoryPicture>().GetObject(x => x.Category.SeoUrl == SeoUrl);
            var fullPath = Server.MapPath("~/Uploads/" + model.Picture.PicturePath);

            if (model.Picture.PicturePath == _categoryPictureService.GetDefaultImage())
                return Json(model.Picture.PicturePath, JsonRequestBehavior.AllowGet);

            if (!System.IO.File.Exists(fullPath)) return Json(model.Picture.PicturePath, JsonRequestBehavior.AllowGet);

            System.IO.File.Delete(fullPath);
            model.Picture.PicturePath = _categoryPictureService.GetDefaultImage();
            _unitOfWork.GetRepo<CategoryPicture>().Update(model);
            _unitOfWork.Commit();
            return Json(model.Picture.PicturePath, JsonRequestBehavior.AllowGet);
        }

    }
}