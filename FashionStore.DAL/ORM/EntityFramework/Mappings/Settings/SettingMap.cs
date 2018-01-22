using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Settings
{
    public class SettingMap : EntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            ToTable("Ayarlar");
            Property(x=>x.Id)
                .HasColumnName("AyarId");
            Property(x => x.CompanyName)
                .HasColumnName("Şirket Adı")
                .IsRequired()
                .IsUnicode();
            Property(x => x.MetaTitle)
                .HasColumnName("Site Başlığı")
                .IsRequired()
                .IsUnicode();
            Property(x => x.MetaDescription)
                .HasColumnName("Seo Açıklama")
                .IsRequired()
                .IsUnicode();
            Property(x => x.FacebookUrl)
                .HasColumnName("Facebook Url")
                .IsOptional()
                .IsUnicode();
            Property(x => x.GoogleUrl)
                .HasColumnName("Google Url")
                .IsOptional()
                .IsUnicode();
            Property(x => x.InstagramUrl)
                .HasColumnName("Instagram Url")
                .IsOptional()
                .IsUnicode();
            Property(x => x.PinterestUrl)
                .HasColumnName("Pinterest Url")
                .IsOptional()
                .IsUnicode();
            Property(x => x.TwitterUrl)
                .HasColumnName("Twitter Url")
                .IsOptional()
                .IsUnicode();
            Property(x => x.Address)
                .HasColumnName("Adres")
                .IsRequired()
                .IsUnicode();
            Property(x => x.City)
                .HasColumnName("Şehir")
                .IsRequired()
                .IsUnicode();
            Property(x => x.Town)
                .HasColumnName("İlçe")
                .IsRequired()
                .IsUnicode();
            Property(x => x.PhoneNumber)
                .HasColumnName("Telefon")
                .IsRequired()
                .IsUnicode();
            Property(x => x.FaxNumber)
                .HasColumnName("Fax")
                .IsOptional()
                .IsUnicode();
            Property(x => x.MapXCoordinate)
                .HasColumnName("Harita X Kordinat")
                .IsOptional()
                .IsUnicode();
            Property(x => x.MapYCoordinate)
                .HasColumnName("Harita Y Kordinat")
                .IsOptional()
                .IsUnicode();
            Property(x => x.AboutUs)
                .HasColumnName("Hakkımızda")
                .HasColumnType("text")
                .IsRequired()
                .IsUnicode();
            Property(x => x.ConfidentialityAgreement)
                .HasColumnName("Gizlilik Sözleşmesi")
                .IsRequired()
                .HasColumnType("text")
                .IsUnicode();
            Property(x => x.TermsAgreement)
                .HasColumnName("Şartlar Sözleşmesi")
                .IsRequired()
                .HasColumnType("text")
                .IsUnicode();
            Property(x => x.SalesContract)
                .HasColumnName("Satış Sözleşmesi")
                .IsRequired()
                .HasColumnType("text")
                .IsUnicode();
        }
    }
}
