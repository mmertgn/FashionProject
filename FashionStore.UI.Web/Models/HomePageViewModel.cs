using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FashionStore.Entity.Entities;

namespace FashionStore.UI.Web.Models
{
    public class HomePageViewModel
    {
        public List<SliderPicture> SliderPictures { get; set; }
        public List<ProductPicture> NewArrivalsProducts { get; set; }
    }
}