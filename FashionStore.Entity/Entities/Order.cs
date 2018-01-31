using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
    public class Order : EntityBase
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int BillingAddressId { get; set; }
        public int? ShippingAddressId { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal OrderTax { get; set; }
        public decimal OrderTotal { get; set; }
        public string ShippingMethod { get; set; }
        public bool IsShipped { get; set; }
        public bool IsApproved { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateTime { get; set; }

        
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}
