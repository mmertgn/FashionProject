using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
    public class SliderPicture : EntityBase
    {
        public int Id { get; set; }
        public int SliderId { get; set; }
        public int PictureId { get; set; }

        public virtual Slider Slider { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
