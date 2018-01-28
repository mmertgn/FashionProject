using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Controllers;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
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
            return View("PageError");
        }

        public ErrorController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}