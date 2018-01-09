using FashionStore.UI.Web.Areas.Admin.Models;
using System.Web.Mvc;
using System.Web.Security;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Controllers;
using FashionStore_BLL.Services.Abstracts;
using Unity.Attributes;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IEncryptor _encryptor;
        public AccountController(IUnitOfWork unitOfWork, IEncryptor encryptor) : base(unitOfWork)
        {
            _encryptor = encryptor;
        }
        // GET: Admin/Login
        public ActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/admin");
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View();
            model.Password = _encryptor.Hash(model.Password);
            if (model.Email == "test@test.com" && model.Password == "58099067CC0E2E309060E94D3ECEA332")
            {
                FormsAuthentication.SetAuthCookie("test@test.com", model.RememberMe);
                return Redirect("/Admin");
            }
            else
            {
                ViewBag.FormResult = "Kullanıcı Adı veya Şifre Hatalı";
                return View();
            }
        }

        //public ActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult Register(RegisterModel model)
        //{
        //    return View();
        //}

        public RedirectResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Admin/Account/Login");
        }

        
    }
}
