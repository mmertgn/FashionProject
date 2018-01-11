using FashionStore.Entity.Entities;
using FluentValidation;

namespace FashionStore_BLL.Services.Validations
{
    public class CustomerLoginValidator : AbstractValidator<Customer>
    {
        public CustomerLoginValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Lütfen e-posta adresinizi giriniz.")
                .NotEmpty().WithMessage("Lütfen e-posta adresinizi giriniz.");
            RuleFor(x => x.Password)
                .Length(6,16).WithMessage("Şifreniz 6-16 karakter uzunluğunda olmalıdır.")
                .NotEmpty().WithMessage("\"Şifre\" alanını boş bıraktınız.");

        }
    }
}
