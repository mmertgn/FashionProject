using FashionStore.Entity.Entities;
using System.Collections.Generic;

namespace FashionStore.UI.Web.Models
{
    public class OrderViewModel
    {
        public List<ShoppingCart> CartItems { get; set; }
        public Address Address { get; set; }
        public Customer Customer { get; set; }
        public decimal SumTotal { get; set; }
    }
}