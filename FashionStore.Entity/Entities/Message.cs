using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
    public class Message : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsReaded { get; set; }
        public string AdminAnswer { get; set; }
    }
}
