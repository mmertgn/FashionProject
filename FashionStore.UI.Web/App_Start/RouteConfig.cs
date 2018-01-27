using System.Web.Mvc;
using System.Web.Routing;

namespace FashionStore.UI.Web
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "CategoryPage",
                url: "urunler/{SeoUrl}",
                defaults: new { controller = "Product", action = "List" },
                namespaces: new[] { "FashionStore.UI.Web.Controllers" }
            );

            //routes.MapRoute(
            //    name: "ParentCategoryPage",
            //    url: "urunler1/{SeoUrl}",
            //    defaults: new { controller = "Product", action = "Detail" },
            //    namespaces: new[] { "FashionStore.UI.Web.Controllers" }
            //);

            routes.MapRoute(
                name: "ProductPages",
                url: "urun-detay/{SeoUrl}",
                defaults: new { controller = "Product", action = "Detail" },
                namespaces: new[] { "FashionStore.UI.Web.Controllers" }
            );

            routes.MapRoute(
                name: "ContentPages",
                url: "icerik/{SeoUrl}",
                defaults: new { controller = "Content", action = "GetPage" },
                namespaces: new[] { "FashionStore.UI.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FashionStore.UI.Web.Controllers" }
            );
        }
    }
}
