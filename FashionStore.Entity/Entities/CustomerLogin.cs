namespace FashionStore.Entity.Entities
{
    public class CustomerLogin : EntityBase
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
