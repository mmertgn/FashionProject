using FashionStore.Entity.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Sliders
{
    public class SliderMap : EntityTypeConfiguration<Slider>
    {
        public SliderMap()
        {
            ToTable("Slider");
            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("SliderId");
            Property(x => x.CategoryId)
                .IsRequired()
                .HasColumnName("KategoriId");
            Property(x => x.SliderTitle)
                .HasColumnName("Slider Başlığı")
                .IsUnicode()
                .IsRequired();
            Property(x => x.SliderTitlePosition)
                .HasColumnName("Slider Başlık Pozisyonu")
                .IsUnicode()
                .HasMaxLength(20);
            Property(x => x.TitleTextColor)
                .HasColumnName("Slider Başlık Rengi")
                .IsUnicode()
                .HasMaxLength(20);
            Property(x => x.IsActive)
                .HasColumnName("AktifMi")
                .IsRequired();
            Property(x => x.SilderDescription)
                .HasColumnName("Slider Açıklama")
                .IsUnicode();
            Property(x => x.DisplayOrder)
                .HasColumnName("Görüntülenme Sırası");
            Property(x => x.CreatedTime)
                .HasColumnName("Oluşturulma Tarihi");
        }
    }
}
