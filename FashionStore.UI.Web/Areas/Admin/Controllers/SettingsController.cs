using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Controllers;
using FashionStore_BLL.Validations.SettingValidations;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    public class SettingsController : BaseController
    {
        public SettingsController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

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
    }
}