using FashionStore.Entity.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Customers
{
    public class CustomerLoginMap : EntityTypeConfiguration<CustomerLogin>
    {
        public CustomerLoginMap()
        {
            ToTable("MüşteriLogin Tablosu");
            

            Property(u => u.CustomerId)
                .IsRequired()
                .HasColumnName("MüşteriId");

            HasRequired(x => x.Customer)
                .WithMany(x => x.CustomerLogins)
                .HasForeignKey(x => x.CustomerId);
        }
    }
}
