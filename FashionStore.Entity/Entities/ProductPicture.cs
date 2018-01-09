using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
    public class ProductPicture : EntityBase
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int PictureId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
