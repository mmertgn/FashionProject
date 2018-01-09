using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Customers
{
    public class CustomerPictureMap : EntityTypeConfiguration<CustomerPicture>
    {
        public CustomerPictureMap()
        {
            ToTable("Müşteri Resimleri");
            HasKey(x => x.Id);
            HasRequired(x => x.Customer)
                .WithMany(x => x.CustomerPictures)
                .HasForeignKey(x => x.CustomerId);

            HasRequired(x => x.Picture)
                .WithMany(x => x.CustomerPictures)
                .HasForeignKey(x => x.PictureId);
        }
    }
}
