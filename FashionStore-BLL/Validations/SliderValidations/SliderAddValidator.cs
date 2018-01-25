using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;
using FluentValidation;

namespace FashionStore_BLL.Validations.SliderValidations
{
   public  class SliderAddValidator : AbstractValidator<Slider>
    {
        public SliderAddValidator()
        {
            RuleFor(x => x.SliderTitle)
                .NotNull().WithMessage("Slider Başlığı boş geçilemez.");
            RuleFor(x => x.SilderDescription)
                .NotNull().WithMessage("Slider Açıklaması boş geçilemez.");
            RuleFor(x => x.SliderTitlePosition)
                .NotNull().WithMessage("Slider Başlık Pozisyonu boş geçilemez.");
            RuleFor(x => x.TitleTextColor)
                .NotNull().WithMessage("Slider Başlık Rengi boş geçilemez.");
            RuleFor(x => x.CategoryId)
                .NotNull().WithMessage("Slider kategorisi boş geçilemez.");
            RuleFor(x => x.DisplayOrder)
                .NotNull().WithMessage("Görüntülenme Sırası boş geçilemez")
                .GreaterThanOrEqualTo(1).WithMessage("Görüntülenme sırası 1'den küçük olamaz.");
        }
    }
}
