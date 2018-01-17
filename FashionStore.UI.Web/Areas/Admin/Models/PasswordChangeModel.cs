using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FashionStore.UI.Web.Areas.Admin.Models
{
    public class PasswordChangeModel
    {
        [Required(ErrorMessage = "Parola Boş geçilemez")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,16}$", ErrorMessage = "Şifreniz en az 1 büyük harf, 1 küçük harf ve 1 sayı içermelidir. Şifreniz 6-16 karakter uzunluğunda olmalıdır.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Parola (Tekrar) Boş geçilemez")]
        [Compare("Password", ErrorMessage = "Parolalar uyuşmuyor")]
        public string PasswordConfirm { get; set; }
    }
}