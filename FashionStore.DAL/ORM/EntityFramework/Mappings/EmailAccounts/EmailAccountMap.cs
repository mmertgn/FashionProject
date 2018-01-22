using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.EmailAccounts
{
    public class EmailAccountMap : EntityTypeConfiguration<EmailAccount>
    {
        public EmailAccountMap()
        {
            HasKey(x => x.Id);
            ToTable("EmailHesabı");
            Property(x => x.Email)
                .IsRequired()
                .IsUnicode();
            Property(x => x.Host)
                .IsRequired()
                .IsUnicode();
            Property(x => x.Port)
                .IsRequired();
            Property(x => x.Username)
                .HasColumnName("Kullanıcı Adı")
                .IsOptional()
                .IsUnicode();
            Property(x => x.Password)
                .HasColumnName("Parola")
                .IsRequired()
                .IsUnicode();
            Property(x => x.EnableSsl)
                .IsRequired();
            Property(x => x.UseDefaultCredentials)
                .IsRequired();
        }
    }
}
