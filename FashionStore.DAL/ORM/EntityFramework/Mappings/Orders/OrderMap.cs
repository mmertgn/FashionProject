using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Orders
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            ToTable("Siparişler");
            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("SiparişId");
            Property(x => x.CustomerId)
                .IsRequired()
                .HasColumnName("MüşteriId");
            Property(x => x.BillingAddressId)
                .IsRequired()
                .HasColumnName("FaturaAdresId");
            Property(x => x.ShippingAddressId)
                .IsOptional()
                .HasColumnName("KargoAdresId");
            Property(x => x.OrderDiscount)
                .HasColumnName("Siparişİndirimi")
                .HasPrecision(16, 2)
                .HasColumnType("decimal")
                .IsRequired();
            Property(x => x.OrderTax)
                .HasColumnName("SiparişVergisi")
                .HasPrecision(16, 2)
                .HasColumnType("decimal")
                .IsRequired();
            Property(x => x.OrderTotal)
                .HasColumnName("ToplamSiparişTutarı")
                .HasPrecision(16, 2)
                .HasColumnType("decimal")
                .IsRequired();
            Property(x => x.ShippingMethod)
                .HasColumnName("KargoGönderimYöntemi")
                .HasColumnType("nvarchar")
                .IsOptional();
            Property(x => x.Deleted)
                .HasColumnName("SilindiMi")
                .IsRequired();
            Property(x => x.CreateTime)
                .HasColumnName("Oluşturulma Tarihi")
                .IsRequired();
        }
    }
}
