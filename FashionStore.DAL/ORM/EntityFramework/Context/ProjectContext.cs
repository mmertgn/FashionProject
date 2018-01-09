using FashionStore.DAL.ORM.EntityFramework.Mappings.Adresses;
using FashionStore.DAL.ORM.EntityFramework.Mappings.Categories;
using FashionStore.DAL.ORM.EntityFramework.Mappings.Customers;
using FashionStore.DAL.ORM.EntityFramework.Mappings.Pictures;
using FashionStore.DAL.ORM.EntityFramework.Mappings.ProductReviews;
using FashionStore.DAL.ORM.EntityFramework.Mappings.Products;
using FashionStore.Entity.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FashionStore.DAL.ORM.EntityFramework.Context
{
    public class ProjectContext : DbContext
    {
        public ProjectContext() : base("name=dbConn")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ProjectContext>());
        }


        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerLogin> CustomerLogins { get; set; }
        public DbSet<CustomerRole> CustomerRoles { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new CategoryMapping());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new CustomerRoleMap());
            modelBuilder.Configurations.Add(new CustomerLoginMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new ProductReviewMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new PictureMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Conventions.Add(new PluralizingTableNameConvention());
        }
    }
}
