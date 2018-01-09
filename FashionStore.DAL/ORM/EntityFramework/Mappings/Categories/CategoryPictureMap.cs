using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Categories
{
    public class CategoryPictureMap : EntityTypeConfiguration<CategoryPicture>
    {
        public CategoryPictureMap()
        {
            ToTable("Kategori Resimleri");
            HasKey(x => x.Id);
            HasRequired(x => x.Category)
                .WithMany(x => x.CategoryPictures)
                .HasForeignKey(x => x.CategoryId);

            HasRequired(x => x.Picture)
                .WithMany(x => x.CategoryPictures)
                .HasForeignKey(x => x.PictureId);
        }
    }
}
