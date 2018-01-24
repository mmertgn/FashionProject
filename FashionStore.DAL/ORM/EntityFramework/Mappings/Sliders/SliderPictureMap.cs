using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Sliders
{
    public class SliderPictureMap : EntityTypeConfiguration<SliderPicture>
    {
        public SliderPictureMap()
        {
            ToTable("Slider Resimleri");
            HasKey(x => x.Id);
            HasRequired(x => x.Slider)
                .WithMany(x => x.SliderPictures)
                .HasForeignKey(x => x.SliderId);

            HasRequired(x => x.Picture)
                .WithMany(x => x.SliderPictures)
                .HasForeignKey(x => x.PictureId);
        }
    }
}
