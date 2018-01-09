using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
    public class CategoryPicture : EntityBase
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int PictureId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
