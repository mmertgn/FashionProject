using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
    public class Setting : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

    }
}
