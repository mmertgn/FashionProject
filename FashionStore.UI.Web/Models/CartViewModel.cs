using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FashionStore.Entity.Entities;

namespace FashionStore.UI.Web.Models
{
    public class CartViewModel
    {
        public CartViewModel()
        {
            CartItems = new List<ShoppingCart>();
        }
        public List<ShoppingCart> CartItems { get; set; }
        public decimal SumTotal { get; set; }
    }
}