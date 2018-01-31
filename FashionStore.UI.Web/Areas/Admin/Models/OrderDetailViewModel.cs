using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FashionStore.Entity.Entities;

namespace FashionStore.UI.Web.Areas.Admin.Models
{
    public class OrderDetailViewModel
    {
        public Order Order { get; set; }
        public Address ShipAddress { get; set; }
        public Address BillAddress { get; set; }
    }
}