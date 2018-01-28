namespace FashionStore.Entity.Entities
{
    public class Wishlist : EntityBase
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }

    }
}
