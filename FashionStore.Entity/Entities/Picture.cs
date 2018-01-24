using System.Collections.Generic;

namespace FashionStore.Entity.Entities
{
    public class Picture : EntityBase
    {
        public int Id { get; set; }
        public string PicturePath { get; set; }
        public string SeoFileName { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }

        public virtual ICollection<ProductPicture> ProductPictures { get; set; }
        public virtual ICollection<CategoryPicture> CategoryPictures { get; set; }
        public virtual ICollection<CustomerPicture> CustomerPictures { get; set; }
        public virtual ICollection<SliderPicture> SliderPictures { get; set; }
    }
}
