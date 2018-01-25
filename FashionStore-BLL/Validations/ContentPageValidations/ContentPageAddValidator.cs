using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FluentValidation;

namespace FashionStore_BLL.Validations.ContentPageValidations
{
    public class ContentPageAddValidator : AbstractValidator<ContentPage>
    {

        private readonly IUnitOfWork _unitOfWork;
        public ContentPageAddValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Sayfa başlığı boş bırakılamaz.")
                .Must(UniqueNameCheck).WithMessage("Bu isimde bir sayfa zaten var.");

            RuleFor(x => x.PageDetail)
                .NotEmpty().WithMessage("Sayfa içeriği boş geçilemez.");

        }

        private bool UniqueNameCheck(ContentPage content, string name)
        {
            var data = _unitOfWork.GetRepo<ContentPage>().Where(x => x.Title == name ).FirstOrDefault();

            if (data == null)
            {
                return true;
            }

            return false;
        }
    }
}
