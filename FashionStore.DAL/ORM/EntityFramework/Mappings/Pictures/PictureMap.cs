﻿using FashionStore.Entity.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Pictures
{
    public class PictureMap :EntityTypeConfiguration<Picture>
    {
        public PictureMap()
        {
            ToTable("Resimler");
            HasKey(x => x.Id);
            Property(x => x.Id)
                .HasColumnName("ResimId");
            Property(x => x.ProductId)
                .HasColumnName("ÜrünId");
            Property(x=>x.PicturePath)
                .HasColumnName("ResimYolu")
                .IsRequired();
            HasRequired(x => x.Product)
                .WithMany(x => x.Pictures)
                .HasForeignKey(x => x.ProductId);
        }
    }
}
