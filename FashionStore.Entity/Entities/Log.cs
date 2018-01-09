using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
    public class Log : EntityBase
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int CustomerId { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string Description { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
