using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public ActionResult Index()
        {
            ViewBag.SirketAdi = "Elsa Giyim";
            return View();
        }

        public PartialViewResult UserProfile()
        {
            var customerModel = _unitOfWork.GetRepo<CustomerPicture>().Where(x => x.Customer.Email == HttpContext.User.Identity.Name).FirstOrDefault();
            return PartialView("_PartialUserProfile",customerModel);
        }
        public PartialViewResult Sidebar()
        {
            var model = _unitOfWork.GetRepo<AdminMenuBar>().Where(x=>x.ParentSidebarId==null).OrderBy(x=>x.DisplayOrder).ToList();
            return PartialView("_PartialSidebar",model);
        }

    }
}