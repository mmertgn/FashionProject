using FashionStore.Entity.Entities;
using FluentValidation;

namespace FashionStore_BLL.Validations.PasswordValidations
{
    public class PasswordResetValidator : AbstractValidator<Customer>
    {
        public PasswordResetValidator(string PasswordConfirm)
        {
            RuleFor(x => x.Password)
                .Equal(PasswordConfirm).WithMessage("Şifreler uyuşmamaktadır.")
                .NotEmpty().WithMessage("Şifre alanı boş bırakılamaz.")
                .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,16}$").WithMessage("Şifreniz en az 1 büyük harf, 1 küçük harf ve 1 sayı içermelidir. Şifreniz 6-16 karakter uzunluğunda olmalıdır.");
        }
    }
}
