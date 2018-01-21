using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;
using FluentValidation;

namespace FashionStore_BLL.Validations.SettingValidations
{
    public class AdminMenuAddValidator : AbstractValidator<AdminMenuBar>
    {
        public AdminMenuAddValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Menu adı alanı boş bırakılamaz.");
            RuleFor(x => x.ControllerActionName)
                .NotNull().WithMessage("Menu Yolu alanı boş bırakılamaz.");
            RuleFor(x => x.DisplayOrder)
                .NotNull().WithMessage("Görüntülenme sırası alanı boş geçilemez.")
                .GreaterThanOrEqualTo(1).WithMessage("Görüntülenme sırası 1'den küçük olamaz.");
        }
    }
}
