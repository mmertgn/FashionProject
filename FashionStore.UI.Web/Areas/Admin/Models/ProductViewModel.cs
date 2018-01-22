using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FashionStore.Entity.Entities;

namespace FashionStore.UI.Web.Areas.Admin.Models
{
    public class ProductViewModel
    {
        public List<ProductPicture> ProductPictures { get; set; }
        public Product Product { get; set; }
        public List<HttpPostedFileBase> PostedPictures { get; set; }
    }
}