using System.Data.Entity.ModelConfiguration;
using FashionStore.Entity.Entities;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Products
{
    public class ProductPictureMap : EntityTypeConfiguration<ProductPicture>
    {
        public ProductPictureMap()
        {
            ToTable("Ürün Resimleri");
            HasKey(x => x.Id);
            HasRequired(x => x.Product)
                .WithMany(x => x.ProductPictures)
                .HasForeignKey(x => x.ProductId);

            HasRequired(x => x.Picture)
                .WithMany(x => x.ProductPictures)
                .HasForeignKey(x => x.PictureId);
        }
    }
}
