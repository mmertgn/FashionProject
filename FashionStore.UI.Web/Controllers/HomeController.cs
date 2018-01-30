using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using FashionStore_BLL.Services.Abstracts;
using PagedList;

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
        public PartialViewResult Cart()
        {
            var model = new CartViewModel
            {
                CartItems = _unitOfWork.GetRepo<ShoppingCart>().Where(x => x.Customer.Email == User.Identity.Name).ToList()
            };
            model.SumTotal = model.CartItems.Count > 0 ? model.CartItems.Sum(x => x.Product.Price * x.Quantity/* * 1.18M*/) : 0M;
            return PartialView("_PartialCart", model);
        }
        [CustomAuthorization(Roles = "Admin,Üye")]
        public ActionResult DeleteFromCart(string SeoUrl)
        {
            var cartItem = _unitOfWork.GetRepo<ShoppingCart>()
                .GetObject(x => x.Product.SeoUrl == SeoUrl && x.Customer.Email == User.Identity.Name);
            _unitOfWork.GetRepo<ShoppingCart>().Delete(cartItem.Id);
            _unitOfWork.Commit();
            return RedirectToAction("CartList", "Product");
        }
        [CustomAuthorization(Roles = "Admin,Üye")]
        public ActionResult CartAdd(int Quantity, int ProductId, string returnUrl)
        {
            var cartItem = _unitOfWork.GetRepo<ShoppingCart>()
                .Where(x => x.ProductId == ProductId && x.Customer.Email == User.Identity.Name).FirstOrDefault();
            if (cartItem != null)
            {
                cartItem.Quantity = cartItem.Quantity + Quantity;
                _unitOfWork.GetRepo<ShoppingCart>().Update(cartItem);
            }
            else
            {
                var model = new ShoppingCart
                {
                    ProductId = ProductId,
                    Quantity = Quantity,
                    CustomerId = _unitOfWork.GetRepo<Customer>().GetObject(x => x.Email == User.Identity.Name).Id,
                    CreateTime = DateTime.Now
                };
                _unitOfWork.GetRepo<ShoppingCart>().Add(model);
            }

            _unitOfWork.Commit();
            return Redirect(returnUrl);
        }

        public ActionResult SearchBox(string Text, int? page)
        {
            Text = Text ?? "";
            var pageIndex = page ?? 1;
            var productList = _unitOfWork.GetRepo<Product>().Where(x => x.Name.Contains(Text) && !x.Deleted && x.Active).OrderBy(x => x.DisplayOrder).ToPagedList(pageIndex, 12);
            ViewBag.Count = _unitOfWork.GetRepo<Product>().Where(x => x.Name.Contains(Text) && !x.Deleted && x.Active).Count();
            ViewBag.SearchedText = Text;
            var model = new ProductListViewModel
            {
                Products = productList
            };
            const bool isSuccess = true;
            TempData["IsSuccess"] = isSuccess;
            TempData["ModelState"] = ModelState;
            TempData["Message"] = "Aranılan kelime '"+Text+"'. Arama sonucu "+ productList.Count() + " ürün bulunmuştur.";
            return View(model);
        }
    }
}