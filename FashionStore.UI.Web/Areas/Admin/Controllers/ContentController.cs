using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Areas.Admin.Models;
using FashionStore.UI.Web.Controllers;
using FashionStore_BLL.Services.Abstracts;
using FashionStore_BLL.Validations.ContentPageValidations;
using FashionStore_BLL.Validations.FaqValidations;
using FashionStore_BLL.Validations.SliderValidations;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity.Attributes;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class ContentController : BaseController
    {
        private readonly IUploadService _uploadService;
        private readonly ISeoUrlMaker _seoUrlMaker;
        public ContentController(IUnitOfWork unitOfWork,
            ISeoUrlMaker seoUrlMaker,
            [Dependency("PhotoUpload")]IUploadService uploadService) : base(unitOfWork)
        {
            _uploadService = uploadService;
            _seoUrlMaker = seoUrlMaker;
        }
        #region Sliderİşlemleri
        public ActionResult SliderList()
        {
            var model = _unitOfWork.GetRepo<Slider>().GetAll().ToList();
            return View(model);
        }
        public ActionResult SliderAdd()
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);

            var categories = _unitOfWork.GetRepo<Category>().Where(x => x.ParentCategoryId != null).ToList();
            var clist = categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.ParentCategory.Name + " >> " + x.Name
            });
            ViewBag.Categories = clist;
            var model = new SliderViewModel()
            {
                Slider = new Slider()
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SliderAdd(SliderViewModel model)
        {
            var validator = new SliderAddValidator().Validate(model.Slider);
            if (validator.IsValid)
            {
                model.Slider.CreatedTime = DateTime.Now;
                _unitOfWork.GetRepo<Slider>().Add(model.Slider);
                var picture = new Picture();
                var picturePath = _uploadService.Upload(model.PostedSlider);
                picture.PicturePath = picturePath;
                picture.SeoFileName = _seoUrlMaker.MakeSlug(model.Slider.SliderTitle);
                picture.TitleAttribute = _seoUrlMaker.MakeSlug(model.Slider.SliderTitle);
                picture.AltAttribute = model.Slider.SliderTitle;
                _unitOfWork.GetRepo<Picture>().Add(picture);
                _unitOfWork.GetRepo<SliderPicture>().Add(new SliderPicture()
                {
                    Slider = model.Slider,
                    Picture = picture
                });
            }
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError("Slider." + a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Slider ekleme işlemi başarılı bir şekilde gerçekleştirildi." : "Slider ekleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";

            return RedirectToAction("SliderAdd");
        }
        public ActionResult SliderEdit(int id)
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
            var categories = _unitOfWork.GetRepo<Category>().Where(x => x.ParentCategoryId != null).ToList();
            var clist = categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.ParentCategory.Name + " >> " + x.Name
            });
            ViewBag.Categories = clist;

            var sliderModel = _unitOfWork.GetRepo<SliderPicture>().GetObject(x => x.Slider.Id == id);
            return View(sliderModel);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SliderEdit(SliderPicture model)
        {
            var validator = new SliderUpdateValidator().Validate(model.Slider);
            if (validator.IsValid)
            {
                _unitOfWork.GetRepo<Slider>().Update(model.Slider);
            }

            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError("Slider." + a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Slider içeriği güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Slider içeriği güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("SliderEdit", new { model.Slider.Id });
        }
        public ActionResult SliderDelete(int id)
        {
            _unitOfWork.GetRepo<SliderPicture>().Delete(id);
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            TempData["Message"] = isSuccess ? "Slider silme işlemi başarılı bir şekilde gerçekleştirildi." : "Slider silme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("SliderList");
        }
        [HttpPost]
        public JsonResult SliderPhotoUpload(HttpPostedFileBase sliderphoto, int Id)
        {
            if (sliderphoto == null) return Json("");

            var model = _unitOfWork.GetRepo<SliderPicture>().GetObject(x => x.SliderId == Id);

            var fullPath = Server.MapPath("~/Uploads/" + model.Picture.PicturePath);

            if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);

            var picturePath = _uploadService.Upload(sliderphoto);
            model.Picture.PicturePath = picturePath;
            _unitOfWork.GetRepo<SliderPicture>().Update(model);

            var isSuccess = _unitOfWork.Commit();
            ViewBag.IsSuccess = isSuccess;
            ViewBag.Message = isSuccess ? "Slider resmi başarılı bir şekilde güncelleştirildi." : "Slider resmi güncelleştirilemedi, lütfen tekrar deneyiniz.";
            return Json(picturePath);
        }
        #endregion

        #region SıkçaSorulanSorularİşlemleri
        public ActionResult FaqList()
        {
            var model = _unitOfWork.GetRepo<FrequantlyQuestion>().GetAll().ToList();
            return View(model);
        }

        public ActionResult FaqAdd()
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
            return View(new FrequantlyQuestion());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult FaqAdd(FrequantlyQuestion model)
        {
            var validator = new FaqValidator().Validate(model);
            if (validator.IsValid)
            {
                _unitOfWork.GetRepo<FrequantlyQuestion>().Add(model);
            }

            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError(a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Soru-Cevap ekleme işlemi başarılı bir şekilde gerçekleştirildi." : "Soru-Cevap ekleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";

            return RedirectToAction("FaqAdd");
        }
        public ActionResult FaqEdit(int id)
        {
            var model = _unitOfWork.GetRepo<FrequantlyQuestion>().GetObject(x => x.Id == id);
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult FaqEdit(FrequantlyQuestion model)
        {
            var validator = new FaqValidator().Validate(model);
            if (validator.IsValid)
            {
                _unitOfWork.GetRepo<FrequantlyQuestion>().Update(model);
            }

            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError(a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Soru-Cevap güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Soru-Cevap güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("FaqEdit", new { model.Id });
        }
        public ActionResult FaqDelete(int id)
        {
            _unitOfWork.GetRepo<FrequantlyQuestion>().Delete(id);
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            TempData["Message"] = isSuccess ? "Soru-Cevap silme işlemi başarılı bir şekilde gerçekleştirildi." : "Soru-Cevap silme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("FaqList");
        }
        #endregion

        #region İçerikSayfasıİşlemleri

        public ActionResult ContentPageList()
        {
            var model = _unitOfWork.GetRepo<ContentPage>().GetAll();
            return View(model);
        }

        public ActionResult ContentPageAdd()
        {
            if (TempData["ModelState"] != null && !ModelState.Equals(TempData["ModelState"]))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
            return View(new ContentPage());
        }
        [HttpPost,ValidateAntiForgeryToken,ValidateInput(false)]
        public ActionResult ContentPageAdd(ContentPage model)
        {
            var validator = new ContentPageAddValidator(_unitOfWork).Validate(model);
            if (validator.IsValid)
            {
                model.SeoUrl = _seoUrlMaker.MakeSlug(model.Title);
                _unitOfWork.GetRepo<ContentPage>().Add(model);
            }

            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError(a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Sayfa ekleme işlemi başarılı bir şekilde gerçekleştirildi." : "Sayfa ekleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";

            return RedirectToAction("ContentPageAdd");
        }

        public ActionResult ContentPageEdit(int id)
        {
            var model = _unitOfWork.GetRepo<ContentPage>().GetObject(x => x.Id == id);
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult ContentPageEdit(ContentPage model)
        {
            var validator = new ContentPageUpdateValidator(_unitOfWork).Validate(model);
            if (validator.IsValid)
            {
                _unitOfWork.GetRepo<ContentPage>().Update(model);
            }

            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            validator.Errors.ToList().ForEach(a =>
            {
                ModelState.AddModelError(a.PropertyName, a.ErrorMessage);
            });
            TempData["ModelState"] = ModelState;
            TempData["Message"] = isSuccess ? "Sayfa güncelleme işlemi başarılı bir şekilde gerçekleştirildi." : "Sayfa güncelleme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("ContentPageEdit", new { model.Id });
        }
        public ActionResult ContentPageDelete(int id)
        {
            _unitOfWork.GetRepo<ContentPage>().Delete(id);
            var isSuccess = _unitOfWork.Commit();
            TempData["IsSuccess"] = isSuccess;
            TempData["Message"] = isSuccess ? "Sayfa silme işlemi başarılı bir şekilde gerçekleştirildi." : "Sayfa silme işlemi gerçekleştirilemedi lütfen tekrar deneyiniz.";
            return RedirectToAction("ContentPageList");
        }
        #endregion
    }
}