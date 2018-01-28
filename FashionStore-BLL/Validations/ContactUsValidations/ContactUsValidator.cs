using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;
using FluentValidation;

namespace FashionStore_BLL.Validations.ContactUsValidations
{
    public class ContactUsValidator : AbstractValidator<Message>
    {
        public ContactUsValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("İsim soyisim alanı boş geçilemez.")
                .NotEmpty().WithMessage("İsim soyisim alanı boş geçilemez.");
            RuleFor(x => x.Mail)
                .NotNull().WithMessage("Email alanı boş geçilemez.")
                .NotEmpty().WithMessage("Email alanı boş geçilemez.");
            RuleFor(x => x.Subject)
                .NotNull().WithMessage("Konu alanı boş geçilemez.")
                .NotEmpty().WithMessage("Konu alanı boş geçilemez.");
            RuleFor(x => x.Comment)
                .NotNull().WithMessage("Mesaj alanı boş geçilemez.")
                .NotEmpty().WithMessage("Mesaj alanı boş geçilemez.");

        }
    }
}
