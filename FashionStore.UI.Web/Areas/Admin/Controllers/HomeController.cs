using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Controllers;
using FashionStore_BLL.Services.Abstracts;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IEncryptor _encryptor;
        public HomeController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public ActionResult Index()
        {
            return View();
        }

        
    }
}