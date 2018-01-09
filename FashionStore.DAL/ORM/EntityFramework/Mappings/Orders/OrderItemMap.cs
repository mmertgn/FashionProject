using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Orders
{
    public class OrderItemMap : EntityTypeConfiguration<OrderItem>
    {
        public OrderItemMap()
        {
            ToTable("SiparişÜrünler");
            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("SiparişÜrünId");
            Property(x => x.OrderId)
                .HasColumnName("SiparişId")
                .IsRequired();
            Property(x => x.ProductId)
                .HasColumnName("ÜrünId")
                .IsRequired();
            Property(x => x.Quantity)
                .HasColumnName("Miktar")
                .IsRequired();

        }
    }
}
