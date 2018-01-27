using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
    public class ContentPage : EntityBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PageDetail { get; set; }
        public string SeoUrl { get; set; }
        public bool ShowOnHeader { get; set; }
        public bool ShowOnFooterColumn1 { get; set; }
        public bool ShowOnFooterColumn2 { get; set; }
        public bool ShowOnFooterColumn3 { get; set; }
        public int DisplayOrder { get; set; }
    }
}
