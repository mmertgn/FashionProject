using FashionStore.UI.Web.Areas.Admin.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace FashionStore.UI.Web.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
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
            if (ModelState.IsValid)
            {
                if (model.Email == "test@test.com" && model.Password == "Sezo9095")
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
            return View();
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
