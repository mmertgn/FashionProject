using FashionStore.Entity.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Products
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            ToTable("Ürün Tablosu");

            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("ÜrünId");
            Property(x => x.CategoryId)
                .IsRequired()
                .HasColumnName("KategoriId");

            Property(x => x.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(255)
                .HasColumnName("Ürün Adı");

            Property(x => x.ShortDescription)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(255)
                .HasColumnName("Kısa Açıklama");

            Property(x => x.FullDescription)
                .IsRequired()
                .IsUnicode()
                .HasColumnName("Uzun Açıklama");

            Property(x => x.AdminComment)
                .IsOptional()
                .IsUnicode()
                .HasColumnName("Yönetici Yorumu");

            Property(x => x.ShowOnHomePage)
                .HasColumnName("AnasayfadaGöster");

            Property(x => x.MetaKeywords)
                .IsOptional()
                .IsUnicode()
                .HasColumnName("Seo Kelimeler");

            Property(x => x.MetaDescription)
                .IsOptional()
                .IsUnicode()
                .HasColumnName("Seo Açıklama");

            Property(x => x.MetaTitle)
                .IsOptional()
                .IsUnicode()
                .HasColumnName("Seo Başlık");

            Property(x => x.StockQuantity)
                .IsRequired()
                .HasColumnName("Stok Miktarı");

            Property(x => x.OrderMinimumQuantity)
                .IsOptional()
                .HasColumnName("MinSiparişMiktarı");

            Property(x => x.OrderMaximumQuantit)
                .IsOptional()
                .HasColumnName("MaxSiparişMiktarı");

            Property(x => x.Price)
                .HasPrecision(16, 2)
                .HasColumnName("Fiyat")
                .HasColumnType("decimal")
                .IsRequired();

            Property(x => x.OldPrice)
                .HasPrecision(16, 2)
                .HasColumnName("EskiFiyat")
                .HasColumnType("decimal")
                .IsOptional();

            Property(x => x.IsFeaturedProduct)
                .HasColumnName("ÖneÇıkarılanÜrün");

            Property(x => x.MarkAsNew)
                .HasColumnName("YeniOlarakİşaretle");

            Property(x => x.MarkAsNewStartTime)
                .IsOptional()
                .HasColumnName("YeniÜrünBaşlangıç");
            Property(x => x.MarkAsNewEndTime)
                .IsOptional()
                .HasColumnName("YeniÜrünBitiş");

            Property(x => x.AvailableStartDateTime)
                .IsOptional()
                .HasColumnName("ErişimAçılmaSaati");
            Property(x => x.AvailableEndDateTime)
                .IsOptional()
                .HasColumnName("ErişimeKapanmaSaati");

            Property(x => x.Active)
                .HasColumnName("ErişimDurumu");

            Property(x => x.Deleted)
                .HasColumnName("SilindiMi");

            Property(x => x.DisplayOrder)
                .HasColumnName("GörüntülenmeSırası");

            Property(x => x.CreateTime)
                .IsRequired()
                .HasColumnName("Oluşturulma Tarihi");
            Property(x => x.UpdateTime)
                .IsOptional()
                .HasColumnName("SonGüncellemeTarihi");

            HasRequired(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
