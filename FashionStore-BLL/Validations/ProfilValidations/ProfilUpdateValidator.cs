using System.Linq;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FluentValidation;

namespace FashionStore_BLL.Validations.ProfilValidations
{
    public class ProfilUpdateValidator : AbstractValidator<Customer>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProfilUpdateValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim alanı boş bırakılamaz.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyad alanı boş bırakılamaz.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
                .EmailAddress().WithMessage("Lütfen e-posta adresinizi giriniz.")
                .Must(UniqueEmail).WithMessage("Bu Email adresi zaten kayıtlı.");

        }

        private bool UniqueEmail(Customer customer, string email)
        {
            var model = _unitOfWork.GetRepo<Customer>().Where(x => x.Id != customer.Id && x.Email == email && x.Deleted == false).FirstOrDefault();

            if (model == null)
            {
                return true;
            }

            return false;
        }

    }
}
