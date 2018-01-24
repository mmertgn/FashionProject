using System.Data.Entity.ModelConfiguration;
using FashionStore.Entity.Entities;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Contents
{
    public class QuestionMap : EntityTypeConfiguration<FrequantlyQuestion>
    {
        public QuestionMap()
        {
            ToTable("Sıkça Sorulan Sorular");
            HasKey(x => x.Id);
            Property(x => x.Question)
                .HasColumnName("Soru")
                .IsUnicode()
                .IsRequired();
            Property(x => x.Answer)
                .HasColumnName("Cevap")
                .IsUnicode()
                .IsRequired();
            Property(x => x.IsColumn1)
                .HasColumnName("FooterSütun1GözükmeDurumu");
            Property(x => x.IsColumn2)
                .HasColumnName("FooterSütun2GözükmeDurumu");
            Property(x => x.IsColumn3)
                .HasColumnName("FooterSütun3GözükmeDurumu");

        }
    }
}
