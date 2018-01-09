using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Core.Database;
using FashionStore.Entity.Entities;

namespace FashionStore.Repository.Repositories.Abstracts
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
        IRepository<T> GetRepo<T>() where T : EntityBase, new();
        void Dispose(bool disposing);

    }
}
