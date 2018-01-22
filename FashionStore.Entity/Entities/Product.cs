using System;
using System.Collections.Generic;

namespace FashionStore.Entity.Entities
{
    public class Product : EntityBase
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string AdminComment { get; set; }
        public bool ShowOnHomePage { get; set; }
        public string SeoUrl { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public int StockQuantity { get; set; }
        public int? OrderMinimumQuantity { get; set; }
        public int? OrderMaximumQuantit { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public bool IsFeaturedProduct { get; set; }
        public bool MarkAsNew { get; set; }
        public DateTime? MarkAsNewStartTime { get; set; }
        public DateTime? MarkAsNewEndTime { get; set; }
        public DateTime? AvailableStartDateTime { get; set; }
        public DateTime? AvailableEndDateTime { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public virtual ICollection<ProductReview> ProductReviews { get; set; }

        public virtual ICollection<ProductPicture> ProductPictures { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public virtual Category Category { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
