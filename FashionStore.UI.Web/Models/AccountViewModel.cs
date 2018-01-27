using FashionStore.Entity.Entities;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.UI.Web.Models
{
    public class AccountViewModel
    {
        public LoginModel LoginModel { get; set; }
        public Customer Customer { get; set; }
        public string PasswordConfirm { get; set; }
    }
    public class LoginModel
    {
        [Required(ErrorMessage = "E-Posta Boş geçilemez.")]
        [EmailAddress(ErrorMessage = "E-posta formatında giriş yapınız.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Parola Boş geçilemez.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,16}$",ErrorMessage = "Şifreniz en az 1 büyük harf, 1 küçük harf ve 1 sayı içermelidir. Şifreniz 6-16 karakter uzunluğunda olmalıdır.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    

}