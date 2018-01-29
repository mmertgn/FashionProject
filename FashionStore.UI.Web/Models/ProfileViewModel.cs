using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FashionStore.Entity.Entities;

namespace FashionStore.UI.Web.Models
{
    public class ProfileViewModel
    {
        public Customer Customer { get; set; }
        public HttpPostedFileBase ProfilePhoto { get; set; }
        public Address Address { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}