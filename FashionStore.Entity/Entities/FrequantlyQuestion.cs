using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
    public class FrequantlyQuestion : EntityBase
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

    }
}
