using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace FashionStore.UI.Web.Controllers
{
    [CustomAuthorization(Roles = "Admin,Üye")]
    public class OrderController : BaseController
    {
        public OrderController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        [CustomAuthorization(Roles = "Admin,Üye")]
        public ActionResult Checkout()
        {
            var user = _unitOfWork.GetRepo<Customer>().GetObject(x => x.Email == User.Identity.Name);
            var model = new OrderViewModel
            {
                CartItems = _unitOfWork.GetRepo<ShoppingCart>().Where(x => x.Customer.Email == User.Identity.Name).ToList(),
                Customer = user
            };
            model.SumTotal = model.CartItems.Count > 0 ? model.CartItems.Sum(x => x.Product.Price * x.Quantity) : 0M;
            var adresses = _unitOfWork.GetRepo<Address>().Where(x => x.Customer.Email == User.Identity.Name).ToList();
            var shipAddressId = user.ShippingAddress?.Id ?? 0;
            var billAddressId = user.BillingAddress?.Id ?? 0;
            ViewBag.ShippingAdresses = new SelectList(adresses, "Id", "Address1", shipAddressId);
            ViewBag.BillingAdresses = new SelectList(adresses, "Id", "Address1", billAddressId);
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Checkout(OrderViewModel model)
        {
            return View();
        }

        public JsonResult GetShipAddress(int ShipId)
        {
            var address = _unitOfWork.GetRepo<Address>().GetObject(x => x.Id == ShipId);
            var model = new AddressJsonModel
            {
                FullName = address.FirstName + " " + address.LastName,
                Email = address.Email,
                PhoneNumber = address.PhoneNumber,
                CityTown = address.City + " / " + address.Town,
                Address1 = address.Address1,
                Address2 = address.Address2,
                PostalCode = address.PostalCode
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult OrderComplete(OrderViewModel model)
        {
            var cartItems = _unitOfWork.GetRepo<ShoppingCart>().Where(x => x.Customer.Email == User.Identity.Name)
                .ToList();
            var order = new Order
            {
                CustomerId = _unitOfWork.GetRepo<Customer>().GetObject(x => x.Email == User.Identity.Name).Id,
                BillingAddressId = model.Customer.BillingAddress.Id,
                ShippingAddressId = model.Customer.ShippingAddress.Id,
                OrderDiscount = 0,
                OrderTax = 0,
                OrderTotal = cartItems.Count > 0 ? cartItems.Sum(x => x.Product.Price * x.Quantity) : 0M,
                ShippingMethod = "MNG Kargo",
                CreateTime = DateTime.Now
            };
            _unitOfWork.GetRepo<Order>().Add(order);
            
            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    Order = order,
                    Quantity = cartItem.Quantity,
                    ProductId = cartItem.ProductId,
                };
                _unitOfWork.GetRepo<OrderItem>().Add(orderItem);
            }

            var IsSucceed = _unitOfWork.Commit();
            if (!IsSucceed) return RedirectToAction("Checkout");
            foreach (var cartItem in cartItems)
            {
                _unitOfWork.GetRepo<ShoppingCart>().Delete(cartItem.Id);
            }
            _unitOfWork.Commit();

            TempData["Title"] = "Siparişiniz Tamamlandı";
            TempData["Description"] = "Siparişiniz başarıyla alınmıştır. Siparişi onaylanma işlemi ardından tarafınıza gönderimi sağlanacaktır.";
            return RedirectToAction("OrderResult");

        }

        public ActionResult OrderResult()
        {
            return View();
        }
        private class AddressJsonModel
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string CityTown { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string PostalCode { get; set; }
        }
    }
}