using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
   public class ShoppingCart : EntityBase
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
        
    }
}
