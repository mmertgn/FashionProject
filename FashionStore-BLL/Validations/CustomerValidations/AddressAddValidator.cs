using FashionStore.Entity.Entities;
using FluentValidation;

namespace FashionStore_BLL.Validations.CustomerValidations
{
    public class AddressAddValidator : AbstractValidator<Address>
    {
        public AddressAddValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("İsim alanı boş bırakılamaz.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad alanı boş bırakılamaz.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
                .EmailAddress().WithMessage("Lütfen e-posta adresinizi giriniz.");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon alanı boş bırakılamaz.")
                .MaximumLength(11).WithMessage("Telefon numaranızı lütfen (05559876543) biçiminde giriniz.");
            RuleFor(x => x.Town).NotNull().WithMessage("İl alanı boş bırakılamaz.");
            RuleFor(x => x.City).NotEmpty().WithMessage("İlçe alanı boş bırakılamaz.");
            RuleFor(x => x.Address1).NotEmpty().WithMessage("Adres alanı boş bırakılamaz.");
        }
    }
}
