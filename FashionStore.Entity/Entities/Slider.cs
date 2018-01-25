using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
    public class Slider : EntityBase
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string SliderTitle { get; set; }
        public string SilderDescription { get; set; }
        public string SliderTitlePosition { get; set; }
        public string TitleTextColor { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<SliderPicture> SliderPictures { get; set; }

    }
}
