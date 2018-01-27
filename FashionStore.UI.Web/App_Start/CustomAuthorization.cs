using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FashionStore.DAL.ORM.EntityFramework.Context;
using FashionStore.Repository.Repositories.Abstracts;

namespace FashionStore.UI.Web
{
    public class CustomAuthorization : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //Kullanıcı giriş yapmamışsa login sayfasına at
            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                if (httpContext.Request.Url == null) return base.AuthorizeCore(httpContext);
                var route = httpContext.Request.Url.ToString();
                httpContext.Response.Redirect(route.Contains("Admin") ? "~/Admin/Account/Login" : "~/Account/SignIn");
            }
            else
            {
                var userMail = httpContext.User.Identity.Name;
                using (var context = new ProjectContext())
                {
                    var user = context.Customers.FirstOrDefault(x => x.Email == userMail);
                    var roles = Roles.Split(',');
                    if (user == null) return base.AuthorizeCore(httpContext);
                    if (user.CustomerRole.Name != "Admin")
                    {
                        if (user.CustomerRole.Name != "Üye") return base.AuthorizeCore(httpContext);
                        if (roles.Contains("Üye"))
                            return true;
                    }
                    else
                    {
                        if (roles.Contains("Admin"))
                            return true;
                    }
                }
            }
            return base.AuthorizeCore(httpContext);
        }
    }
}