using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FashionStore.Entity.Entities;

namespace FashionStore.UI.Web.Areas.Admin.Models
{
    public class CustomerEditViewModel
    {
        public Customer Customer { get; set; }
        public List<Address> Addresses { get; set; }
        public PasswordChangeModel PasswordChangeModel { get; set; }
        public Address Address { get; set; }
    }
}