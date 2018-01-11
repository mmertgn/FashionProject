using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
    public class Customer : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public int CustomerRoleId { get; set; }

        public virtual CustomerRole CustomerRole { get; set; }
        public virtual Address BillingAddress { get; set; }
        public virtual Address ShippingAddress { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<ProductReview> ProductReviews { get; set; }
        public virtual ICollection<CustomerLogin> CustomerLogins { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<CustomerPicture> CustomerPictures { get; set; }
    }
}
