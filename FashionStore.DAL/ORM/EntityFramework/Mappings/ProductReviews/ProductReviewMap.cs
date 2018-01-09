using FashionStore.Entity.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.ProductReviews
{
    public class ProductReviewMap : EntityTypeConfiguration<ProductReview>
    {
        public ProductReviewMap()
        {
            ToTable("MüşteriYorum Tablosu");

            HasKey(c => c.Id);
            Property(x => x.Id)
                .HasColumnName("YorumId");

            Property(u => u.CustomerId)
                .IsRequired()
                .HasColumnName("MüşteriId");

            Property(u => u.ProductId)
                .IsRequired()
                .HasColumnName("ÜrünId");

            Property(u => u.IsApproved)
                .IsRequired()
                .HasColumnName("OnaylıMı");

            Property(u => u.Title)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Başlık");

            Property(u => u.ReviewText)
                .IsRequired()
                .IsUnicode()
                .HasColumnName("Yorum");

            Property(u => u.ReplyText)
                .IsOptional()
                .IsUnicode()
                .HasColumnName("CevaplananYorum");

            Property(u => u.Rating)
                .HasColumnName("Puan");

            Property(u => u.HelpfulYesTotal)
                .HasColumnName("YararlıToplam");

            Property(u => u.HelpfulNoTotal)
                .IsRequired()
                .HasColumnName("YararsızToplam");

            Property(u=>u.CreatedTime)
                .HasColumnName("Oluşturulma Tarihi");

            HasRequired(x => x.Product)
                .WithMany(c => c.ProductReviews)
                .HasForeignKey<int>(x => x.ProductId);

            HasRequired(x => x.Customer)
                .WithMany(c => c.ProductReviews)
                .HasForeignKey<int>(x => x.CustomerId);
        }
    }
}
