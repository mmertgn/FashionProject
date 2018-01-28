using System.Web.Mvc;
using FashionStore.Repository.Repositories.Abstracts;

namespace FashionStore.UI.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        public ActionResult Page404(string aspxerrorpath)
        {
            if (!string.IsNullOrEmpty(aspxerrorpath))
                ViewBag.Kaynak = aspxerrorpath;
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            //if (aspxerrorpath.Contains("Admin"))
            //{
            //   return RedirectToAction("Page404","Error"); 
            //}
            return View("Page404");
        }
        public ErrorController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}