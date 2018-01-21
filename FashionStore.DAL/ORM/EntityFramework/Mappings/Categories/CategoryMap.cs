using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using FashionStore.Entity.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Categories
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            ToTable("Kategori Tablosu");

            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("KategoriId");

            Property(x => x.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(75)
                .HasColumnName("Kategori Adı");

            Property(x => x.Description)
                .IsRequired()
                .IsUnicode()
                .HasColumnName("Açıklama");
            Property(x => x.SeoUrl)
                .IsRequired()
                .IsUnicode()
                .HasColumnName("Seo Link");

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

            Property(x => x.ParentCategoryId)
                .IsOptional()
                .HasColumnName("ÜstKategoriId");

            Property(x => x.ShowOnHomePage)
                .HasColumnName("AnasayfadaGöster");

            Property(x => x.ShowOnTopMenu)
                .HasColumnName("ÜstMenudeGöster");

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

            HasOptional(category => category.ParentCategory)
                .WithMany(category => category.ChildCategories)
                .HasForeignKey(category => category.ParentCategoryId);
        }
    }
}
