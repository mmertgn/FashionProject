using System.Collections.Generic;

namespace FashionStore.Entity.Entities
{
    public class CustomerRole : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
