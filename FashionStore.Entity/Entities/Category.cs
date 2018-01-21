using System;
using System.Collections.Generic;

namespace FashionStore.Entity.Entities
{
    public class Category : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SeoUrl { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool ShowOnHomePage { get; set; }
        public bool ShowOnTopMenu { get; set; }
        public bool Deleted { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> ChildCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<CategoryPicture> CategoryPictures { get; set; }
    }
}
