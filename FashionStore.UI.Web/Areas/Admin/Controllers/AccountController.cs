using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.UI.Web.Areas.Admin.Models;
using FashionStore.UI.Web.Controllers;
using FashionStore_BLL.Services.Abstracts;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Unity.Attributes;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly IEncryptor _encryptor;
        public AccountController(IUnitOfWork unitOfWork, [Dependency("MD5")]IEncryptor encryptor) : base(unitOfWork)
        {
            _encryptor = encryptor;
        }
        // GET: Admin/Login
        public ActionResult Login()
        {
            var roleControl = _unitOfWork.GetRepo<Customer>().Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            if (HttpContext.User.Identity.IsAuthenticated && roleControl.CustomerRole.Name != "Üye")
            {
                return Redirect("/Admin/Home");
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            //string hash = Crypto.Hash(model.Password,algorithm:"md5");
            if (!ModelState.IsValid) return View();
            model.Password = _encryptor.Hash(model.Password);
            var customer = _unitOfWork.GetRepo<Customer>().Where(x => x.Email == model.Email && x.Password == model.Password && x.CustomerRole.Name != "Üye").FirstOrDefault();

            if (customer != null)
            {
                FormsAuthentication.SetAuthCookie(customer.Email, model.RememberMe);
                customer.LastLoginTime = DateTime.Now;
                _unitOfWork.Commit();
                return Redirect("/Admin/Home");
            }
            else
            {
                ViewBag.FormResult = "Kullanıcı Adı veya Şifre Hatalı";
                return View();
            }
        }
        [CustomAuthorization(Roles = "Admin")]
        public RedirectResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Admin/Account/Login");
        }


    }
}
