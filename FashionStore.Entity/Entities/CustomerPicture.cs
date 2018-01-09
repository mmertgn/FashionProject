using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
    public class CustomerPicture : EntityBase
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PictureId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
