using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FashionStore.Entity.Entities;
using PagedList;

namespace FashionStore.UI.Web.Models
{
    public class ProductListViewModel
    {
        public List<Category> ParentCategories { get; set; }
        public List<Category> ChildCategories { get; set; }
        public IPagedList<Product> Products { get; set; }
        public Category ChosenCategory { get; set; }
    }
}