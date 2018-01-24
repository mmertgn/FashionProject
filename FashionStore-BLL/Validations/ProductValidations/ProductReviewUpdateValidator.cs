using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;
using FluentValidation;

namespace FashionStore_BLL.Validations.ProductValidations
{
    public class ProductReviewUpdateValidator : AbstractValidator<ProductReview>
    {
        public ProductReviewUpdateValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Yorum Başlığı alanı boş bırakılamaz.");
            RuleFor(x => x.ReviewText).NotEmpty().WithMessage("Yorum İçeriği alanı boş bırakılamaz.");
        }
    }
}
