using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Settings
{
    public class SettingMap : EntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            ToTable("Ayarlar");
            Property(x=>x.Id)
                .HasColumnName("AyarId");
            Property(x => x.Name)
                .HasColumnName("Ayar Adı")
                .IsRequired()
                .IsUnicode();
            Property(x => x.Value)
                .HasColumnName("Ayar Değeri")
                .IsRequired()
                .IsUnicode();

        }
    }
}
