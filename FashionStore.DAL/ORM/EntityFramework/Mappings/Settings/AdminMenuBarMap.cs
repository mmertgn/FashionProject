using FashionStore.Entity.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FashionStore.DAL.ORM.EntityFramework.Mappings.Settings
{
    public class AdminMenuBarMap : EntityTypeConfiguration<AdminMenuBar>
    {
        public AdminMenuBarMap()
        {
            ToTable("AdminMenü");
            HasKey(x => x.Id);
            //HasOptional(category => category.ParentSidebar)
            //    .WithMany(category => category.ChildSidebars)
            //    .HasForeignKey(category => category.ParentSidebarId);
        }
    }
}
