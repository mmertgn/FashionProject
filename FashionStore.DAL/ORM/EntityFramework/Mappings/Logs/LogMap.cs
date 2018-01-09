using FashionStore.Entity.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Logs
{
    public class LogMap : EntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            ToTable("Logs");
            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("LogId");
            Property(x => x.CustomerId)
                .IsRequired()
                .HasColumnName("MüşteriId");
            Property(x => x.ControllerName)
                .IsUnicode()
                .HasMaxLength(100)
                .HasColumnName("Controller Adı");
            Property(x => x.ActionName)
                .IsUnicode()
                .HasMaxLength(100)
                .HasColumnName("Action Adı");
            Property(x => x.CreateTime)
                .IsRequired()
                .HasColumnName("Oluşturulma Tarihi");
            Property(x => x.Description)
                .IsUnicode()
                .HasMaxLength(250)
                .HasColumnName("Açıklama");

            HasRequired(x => x.Customer)
                .WithMany(x => x.Logs)
                .HasForeignKey(x => x.CustomerId);
        }
    }
}
