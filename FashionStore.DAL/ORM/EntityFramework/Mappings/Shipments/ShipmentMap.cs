using FashionStore.Entity.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Shipments
{
    public class ShipmentMap : EntityTypeConfiguration<Shipment>
    {
        public ShipmentMap()
        {
            ToTable("Kargolar");
            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("KargoId");
            Property(x => x.OrderId)
                .IsRequired()
                .HasColumnName("SiparişId");
            Property(x => x.TrackingNumber)
                .HasColumnName("TakipKodu")
                .HasParameterName("nvarchar")
                .IsOptional();
            Property(x => x.ShippedDate)
                .HasColumnName("GönderimTarihi")
                .IsOptional();
            Property(x => x.DeliveryDate)
                .HasColumnName("TeslimTarihi")
                .IsOptional();
            Property(x => x.AdminComment)
                .HasColumnName("YöneticiNotu")
                .HasParameterName("nvarchar")
                .IsOptional();
        }
    }
}
