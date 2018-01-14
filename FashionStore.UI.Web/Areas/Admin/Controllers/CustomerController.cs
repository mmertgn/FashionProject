using System.IO;
using System.Linq;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Controllers;
using System.Web.Mvc;
using FashionStore.Entity.Entities;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class CustomerController : BaseController
    {
        public ActionResult List()
        {
            var model = _unitOfWork.GetRepo<Customer>().GetAll().ToList();
            return View(model);
        }

        public ActionResult ProfileEdit()
        {
            var model = _unitOfWork.GetRepo<CustomerPicture>().Where(x => x.Customer.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult ProfileEdit(CustomerPicture model)
        {
            if (model.Picture.PicturePath != null)
            {
                string dosyaYolu = Path.GetFileName(model.Picture.PicturePath);
                var yuklemeYeri = Path.Combine(Server.MapPath("~/Dosyalar"), dosyaYolu);
                //model.Picture.PicturePath.SaveAs(yuklemeYeri);
            }
            return RedirectToAction("ProfileEdit");
        }

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            return View();
        }
        public CustomerController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}