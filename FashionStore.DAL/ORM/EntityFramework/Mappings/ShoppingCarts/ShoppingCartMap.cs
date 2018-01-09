using FashionStore.Entity.Entities;
using System.Data.Entity.ModelConfiguration;
namespace FashionStore.DAL.ORM.EntityFramework.Mappings.ShoppingCarts
{
    public class ShoppingCartMap : EntityTypeConfiguration<ShoppingCart>
    {
        public ShoppingCartMap()
        {
            ToTable("Sepet");
            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("SepetId");

            Property(x=>x.ProductId)
                .IsRequired()
                .HasColumnName("ÜrünId");

            Property(x => x.CustomerId)
                .IsRequired()
                .HasColumnName("MüşteriId");

            Property(x => x.CreateTime)
                .IsRequired()
                .HasColumnName("Oluşturulma Tarihi");

            Property(x => x.UpdateTime)
                .IsOptional()
                .HasColumnName("SonGüncellemeTarihi");
        }
    }
}
