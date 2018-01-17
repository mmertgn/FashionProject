using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FashionStore.Entity.Entities;

namespace FashionStore.UI.Web.Areas.Admin.Models
{
    public class ProfileEditViewModel
    {
        public CustomerPicture CustomerPicture { get; set; }
        public PasswordChangeModel PasswordChangeModel { get; set; }
    }
}