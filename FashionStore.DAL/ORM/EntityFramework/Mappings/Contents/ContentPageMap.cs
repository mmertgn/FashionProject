using FashionStore.Entity.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Contents
{
    public class ContentPageMap : EntityTypeConfiguration<ContentPage>
    {
        public ContentPageMap()
        {
            ToTable("İçerik Sayfaları");
            Property(x => x.Title)
                .IsRequired()
                .HasColumnName("Sayfa Başlığı");
            Property(x => x.PageDetail)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Sayfa İçeriği");
            Property(x => x.SeoUrl)
                .IsRequired()
                .HasColumnName("Sayfa Linki");
            Property(x => x.ShowOnHeader)
                .HasColumnName("HeaderGörünmeDurumu");
            Property(x => x.ShowOnFooterColumn1)
                .HasColumnName("FooterKolon1GörünmDurumu");
            Property(x => x.ShowOnFooterColumn2)
                .HasColumnName("FooterKolon2GörünmDurumu");
            Property(x => x.ShowOnFooterColumn3)
                .HasColumnName("FooterKolon3GörünmDurumu");
        }
    }
}
