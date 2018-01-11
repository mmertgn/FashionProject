using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore.Entity.Entities
{
    public class AdminMenuBar : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public string IconPath { get; set; }
        public string DataIcon { get; set; }
        public string ControllerActionName { get; set; }
        public int? ParentSidebarId { get; set; }

        public virtual ICollection<AdminMenuBar> ChildSidebars { get; set; }
        public virtual AdminMenuBar ParentSidebar { get; set; }
    }
}
