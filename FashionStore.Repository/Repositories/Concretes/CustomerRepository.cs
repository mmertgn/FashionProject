using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Core.Database;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;

namespace FashionStore.Repository.Repositories.Concretes
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(DbContext context) : base(context)
        {
        }
    }
}
