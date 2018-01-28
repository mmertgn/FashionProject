using System.Data.Entity.ModelConfiguration;
using FashionStore.Entity.Entities;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.WishLists
{
    public class WishListMap : EntityTypeConfiguration<Wishlist>
    {
        public WishListMap()
        {
            ToTable("İstekListesi");
        }
    }
}
