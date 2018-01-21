using System.Web.Mvc;

namespace FashionStore.UI.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Admin";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_category",
                "Admin/Category/{action}/{SeoUrl}",
                new { controller = "Category", action = "List", id = UrlParameter.Optional },
                new[] { "FashionStore.UI.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new {controller="Home", action = "Index", id = UrlParameter.Optional },
                new[] { "FashionStore.UI.Web.Areas.Admin.Controllers" }
            );
        }
    }
}