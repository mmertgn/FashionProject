using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FashionStore.UI.Web.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "E-Posta Boş geçilemez.")]
        [EmailAddress(ErrorMessage = "E-posta formatında giriş yapınız.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Parola Boş geçilemez.")]
        [MinLength(8, ErrorMessage = "En az 6 karakter olmalıdır.")]
        [MaxLength(16, ErrorMessage = "En fazla 16 karakter olmalıdır.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}