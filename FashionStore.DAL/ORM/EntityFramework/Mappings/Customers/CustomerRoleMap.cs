using FashionStore.Entity.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Customers
{
    public class CustomerRoleMap : EntityTypeConfiguration<CustomerRole>
    {
        public CustomerRoleMap()
        {
            ToTable("Müşteri Yetki Tablosu");

            HasKey(cr => cr.Id);

            Property(cr => cr.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
