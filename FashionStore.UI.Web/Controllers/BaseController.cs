using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using System.Linq;
using System.Web.Mvc;

namespace FashionStore.UI.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            var model = _unitOfWork.GetRepo<Setting>().GetAll().FirstOrDefault();
            ViewBag.Title = model.CompanyName;
            ViewBag.SeoDescription = model.MetaDescription;
        }


        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose(disposing);
        }
    }
}