using FashionStore.Entity.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Adresses
{
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            ToTable("Adres Tablosu");

            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("AdresId");

            Property(u => u.FirstName)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50)
                .HasColumnName("Ad");

            Property(u => u.LastName)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50)
                .HasColumnName("Soyad");

            Property(u => u.Email)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50)
                .HasColumnName("Email");

            Property(u => u.Company)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50)
                .HasColumnName("Şirket Adı");

            Property(u => u.CountryId)
                .IsRequired()
                .HasColumnName("ÜlkeId");
            Property(u => u.StateId)
                .IsRequired()
                .HasColumnName("İl");
            Property(u => u.CityId)
                .IsRequired()
                .HasColumnName("İlçe");

            Property(u => u.Address1)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(255)
                .HasColumnName("Adres1");
            Property(u => u.Address2)
                .IsOptional()
                .IsUnicode()
                .HasMaxLength(255)
                .HasColumnName("Adres2");

            Property(u => u.PostalCode)
                .IsOptional()
                .IsUnicode()
                .HasMaxLength(50)
                .HasColumnName("Posta Kodu");

            Property(u => u.PhoneNumber)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(11)
                .HasColumnName("Telefon");

            Property(u => u.CreatedOnUtc)
                .HasColumnName("Oluşturulma Tarihi");

            Property(x => x.CustomerId)
                .HasColumnName("MüşteriId");

            HasRequired(x => x.Customer)
                .WithMany(c => c.Addresses)
                .HasForeignKey(x => x.CustomerId);
        }
    }
}
