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
                name: "DeleteCartItem",
                url: "sepet-sil/{SeoUrl}",
                defaults: new { controller = "Home", action = "DeleteFromCart", page = UrlParameter.Optional },
                namespaces: new[] { "FashionStore.UI.Web.Controllers" }
            );

            routes.MapRoute(
                name: "CategoryPage",
                url: "urunler/{SeoUrl}/{page}",
                defaults: new { controller = "Product", action = "List", page = UrlParameter.Optional },
                namespaces: new[] { "FashionStore.UI.Web.Controllers" }
            );
            
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
