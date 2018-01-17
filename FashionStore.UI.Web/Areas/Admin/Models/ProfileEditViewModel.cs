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

        //[Required(ErrorMessage = "Parola Boş geçilemez")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,16}$", ErrorMessage = "En az 1 harf, 1 numara girilmedir. Şifreniz 6-16 karakter uzunluğunda olmalıdır.")]
        //public string Password { get; set; }
        //[Compare("Password", ErrorMessage = "Parolalar uyuşmuyor")]
        //public string PasswordConfirm { get; set; }
    }
}