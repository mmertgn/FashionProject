using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FluentValidation;

namespace FashionStore_BLL.Validations.CategoryValidations
{
    public class CategoryAddValidator : AbstractValidator<Category>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryAddValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori Adı alanı boş bırakılamaz.")
                .Must(UniqueNameCheck).WithMessage("Bu isimde kategori zaten mevcut.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Kategori Açıklaması alanı boş bırakılamaz.");
            RuleFor(x => x.DisplayOrder)
                .NotEmpty().WithMessage("Görüntülenme Sırası alanı boş bırakılamaz.")
                .GreaterThanOrEqualTo(1).WithMessage("Görüntülenme sırası 1'den küçük olamaz.");
        }
        private bool UniqueNameCheck(Category category, string name)
        {
            var data = _unitOfWork.GetRepo<Category>().Where(x => x.Name == name && x.ParentCategoryId == category.ParentCategoryId && x.Deleted == false).FirstOrDefault();

            if (data == null)
            {
                return true;
            }

            return false;
        }
    }
}
