using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Areas.Admin.Models;
using FashionStore.UI.Web.Controllers;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        public ActionResult ShippedOrderList()
        {
            var orderList = _unitOfWork.GetRepo<Order>().Where(x => x.IsShipped && !x.Deleted).ToList();
            return View("OrderList", orderList);
        }
        public ActionResult ApprovedOrderList()
        {
            var orderList = _unitOfWork.GetRepo<Order>().Where(x => !x.IsApproved && !x.Deleted).ToList();
            return View("OrderList", orderList);
        }
        public ActionResult OrderList()
        {
            var orderList = _unitOfWork.GetRepo<Order>().Where(x => !x.Deleted).ToList();
            return View(orderList);
        }

        public ActionResult Detail(int id)
        {
            var order = _unitOfWork.GetRepo<Order>().GetObject(x => x.Id == id);
            var shipAddress = _unitOfWork.GetRepo<Address>().GetObject(x => x.Id == order.ShippingAddressId);
            var billAddress = _unitOfWork.GetRepo<Address>().GetObject(x => x.Id == order.BillingAddressId);
            var model = new OrderDetailViewModel
            {
                Order = order,
                ShipAddress = shipAddress,
                BillAddress = billAddress
            };
            return View(model);
        }

        public ActionResult OrderDelete(int id)
        {
            var order = _unitOfWork.GetRepo<Order>().GetObject(x => x.Id == id);
            order.Deleted = true;
            _unitOfWork.GetRepo<Order>().Update(order);
            _unitOfWork.Commit();
            return RedirectToAction("OrderList");
        }

        public ActionResult OrderApprove(int id)
        {
            var order = _unitOfWork.GetRepo<Order>().GetObject(x => x.Id == id);
            order.IsApproved = true;
            _unitOfWork.GetRepo<Order>().Update(order);
            _unitOfWork.Commit();
            return RedirectToAction("OrderList");
        }

        public ActionResult OrderShipped(int id)
        {
            var order = _unitOfWork.GetRepo<Order>().GetObject(x => x.Id == id);
            order.IsShipped = true;
            _unitOfWork.GetRepo<Order>().Update(order);
            _unitOfWork.Commit();
            return RedirectToAction("OrderList");
        }
        public OrderController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}