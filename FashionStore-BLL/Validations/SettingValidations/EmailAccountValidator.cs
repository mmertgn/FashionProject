using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;
using FluentValidation;

namespace FashionStore_BLL.Validations.SettingValidations
{
  public  class EmailAccountValidator : AbstractValidator<EmailAccount>
    {
        public EmailAccountValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email alanı boş geçilemez.");
            RuleFor(x => x.Password)
                .NotNull().WithMessage("Şifre alanı boş geçilemez.");
            RuleFor(x => x.Host)
                .NotNull().WithMessage("Host alanı boş geçilemez.");
            RuleFor(x => x.Port)
                .NotNull().WithMessage("Port alanı boş geçilemez.");
        }
    }
}
