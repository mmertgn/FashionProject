using System.Linq;
using System.Web.Mvc;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Models;

namespace FashionStore.UI.Web.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        [CustomAuthorization(Roles = "Admin,Üye")]
        public ActionResult Checkout()
        {
            var model = new OrderViewModel
            {
                CartItems = _unitOfWork.GetRepo<ShoppingCart>().Where(x => x.Customer.Email == User.Identity.Name).ToList()
            };
            model.SumTotal = model.CartItems.Count > 0 ? model.CartItems.Sum(x => x.Product.Price * x.Quantity) : 0M;
            var adresses = _unitOfWork.GetRepo<Address>().Where(x => x.Customer.Email == User.Identity.Name).ToList();
            var user = _unitOfWork.GetRepo<Customer>().GetObject(x => x.Email == User.Identity.Name);
            var shipAddressId = user.ShippingAddress?.Id ?? 0;
            var billAddressId = user.BillingAddress?.Id ?? 0;
            ViewBag.ShippingAdresses = new SelectList(adresses, "Id", "Address1", shipAddressId);
            ViewBag.BillingAdresses = new SelectList(adresses, "Id", "Address1", billAddressId);
            return View(model);
        }


    }
}