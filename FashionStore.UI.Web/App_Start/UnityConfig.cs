using FashionStore.DAL.ORM.EntityFramework.Context;
using FashionStore.Repository.Repositories.Abstracts;
using FashionStore.Repository.Repositories.Concretes;
using System;
using System.Data.Entity;
using FashionStore_BLL.Services.Abstracts;
using FashionStore_BLL.Services.Concretes;
using Unity;

namespace FashionStore.UI.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<DbContext, ProjectContext>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<ISeoUrlMaker, SeoUrlMaker>();
            container.RegisterType<IEncryptor, Md5HashProvider>("MD5");
            container.RegisterType<IPictureService, CustomerPictureService>("CustomerPicture");
            container.RegisterType<IPictureService, CategoryPictureService>("CategoryPicture");
            container.RegisterType<IPictureService, ProductPictureService>("ProductPicture");
            container.RegisterType<ICustomerRepository, CustomerRepository>();
            container.RegisterType<IUploadService, PhotoUploadService>("PhotoUpload");
        }
    }
}