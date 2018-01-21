using FashionStore.DAL.ORM.EntityFramework.Mappings.Adresses;
using FashionStore.DAL.ORM.EntityFramework.Mappings.Categories;
using FashionStore.DAL.ORM.EntityFramework.Mappings.Customers;
using FashionStore.DAL.ORM.EntityFramework.Mappings.EmailAccounts;
using FashionStore.DAL.ORM.EntityFramework.Mappings.Logs;
using FashionStore.DAL.ORM.EntityFramework.Mappings.Orders;
using FashionStore.DAL.ORM.EntityFramework.Mappings.Pictures;
using FashionStore.DAL.ORM.EntityFramework.Mappings.ProductReviews;
using FashionStore.DAL.ORM.EntityFramework.Mappings.Products;
using FashionStore.DAL.ORM.EntityFramework.Mappings.Settings;
using FashionStore.DAL.ORM.EntityFramework.Mappings.Shipments;
using FashionStore.DAL.ORM.EntityFramework.Mappings.ShoppingCarts;
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
        
        #region DbSet
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<CategoryPicture> CategoryPictures { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerLogin> CustomerLogins { get; set; }
        public DbSet<CustomerRole> CustomerRoles { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<EmailAccount> EmailAccounts { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<CustomerPicture> CustomerPictures { get; set; } 
        public DbSet<AdminMenuBar> AdminMenuBars { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        
            #region Mapping
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new CustomerRoleMap());
            modelBuilder.Configurations.Add(new CustomerLoginMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new ProductReviewMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new CategoryPictureMap());
            modelBuilder.Configurations.Add(new PictureMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new ProductPictureMap());
            modelBuilder.Configurations.Add(new ShoppingCartMap());
            modelBuilder.Configurations.Add(new OrderItemMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new ShipmentMap());
            modelBuilder.Configurations.Add(new SettingMap());
            modelBuilder.Configurations.Add(new EmailAccountMap());
            modelBuilder.Configurations.Add(new LogMap());
            modelBuilder.Configurations.Add(new CustomerPictureMap());
            modelBuilder.Configurations.Add(new AdminMenuBarMap());
            modelBuilder.Conventions.Add(new PluralizingTableNameConvention());
            #endregion
        }
    }
}
