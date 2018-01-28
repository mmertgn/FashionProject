using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Messages
{
    public class MessageMap : EntityTypeConfiguration<Message>
    {
        public MessageMap()
        {
            ToTable("GelenKutusu");
            Property(x => x.Name)
                .HasColumnName("Gönderen Adı")
                .IsRequired()
                .IsUnicode();
            Property(x => x.Mail)
                .HasColumnName("GönderenMail")
                .IsUnicode()
                .IsRequired();
            Property(x => x.Subject)
                .HasColumnName("MesajBaşlık")
                .IsUnicode()
                .IsRequired();
            Property(x => x.Comment)
                .HasColumnName("Mesaj İçeriği")
                .IsUnicode()
                .IsRequired();
            Property(x => x.AdminAnswer)
                .HasColumnName("Mesaja Verilen Cevap")
                .IsUnicode()
                .IsOptional();
            Property(x => x.CreatedTime)
                .HasColumnName("Oluşturulma Tarihi")
                .IsOptional();
            Property(x => x.IsReaded)
                .HasColumnName("Okunma Durumu")
                .IsOptional();
        }
    }
}
