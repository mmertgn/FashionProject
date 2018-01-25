using FashionStore.Entity.Entities;
using FluentValidation;

namespace FashionStore_BLL.Validations.SettingValidations
{
    public class GeneralSettingsValidator : AbstractValidator<Setting>
    {
        public GeneralSettingsValidator()
        {

            RuleFor(x => x.Address)
                .NotNull().WithMessage("Adres alanı boş geçilemez.");

            RuleFor(x => x.FaxNumber)
                .NotNull().WithMessage("Fax No alanı boş geçilemez.");
            RuleFor(x => x.MapXCoordinate)
                .NotNull().WithMessage("Harita Kordinat(X) alanı boş geçilemez.");
            RuleFor(x => x.MapYCoordinate)
                .NotNull().WithMessage("Harita Kordinat(Y) alanı boş geçilemez.");
            RuleFor(x => x.PhoneNumber)
                .NotNull().WithMessage("Telefon alanı boş geçilemez.");

            RuleFor(x => x.City)
                .NotNull().WithMessage("İlçe alanı boş geçilemez.");
            RuleFor(x => x.Town)
                .NotNull().WithMessage("Şehir alanı boş geçilemez.");
            RuleFor(x => x.CompanyName)
                .NotNull().WithMessage("Şirket Adı alanı boş geçilemez.");
            RuleFor(x => x.MetaTitle)
                .NotNull().WithMessage("SEO Başlık alanı boş geçilemez.");
            RuleFor(x => x.MetaDescription)
                .NotNull().WithMessage("SEO Açıklama alanı boş geçilemez.");
        }
    }
}
