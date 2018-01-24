using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FashionStore.Entity.Entities;

namespace FashionStore.UI.Web.Areas.Admin.Models
{
    public class SliderViewModel
    {
        public Slider Slider { get; set; }
        public HttpPostedFileBase PostedSlider { get; set; }
    }
}