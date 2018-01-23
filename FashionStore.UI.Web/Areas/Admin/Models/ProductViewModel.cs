using FashionStore.Entity.Entities;
using System.Collections.Generic;
using System.Web;

namespace FashionStore.UI.Web.Areas.Admin.Models
{
    public class ProductViewModel
    {
        public List<ProductPicture> ProductPictures { get; set; }
        public Product Product { get; set; }
        public List<HttpPostedFileBase> PostedPictures { get; set; }
        public int CatId { get; set; }
    }
}