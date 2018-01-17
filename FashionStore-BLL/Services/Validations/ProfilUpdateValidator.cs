using System.Linq;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FluentValidation;

namespace FashionStore_BLL.Services.Validations
{
    public class ProfilUpdateValidator : AbstractValidator<Customer>
    {
        private IUnitOfWork _UnitOfWork;
        public ProfilUpdateValidator(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim alanı boş bırakılamaz.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyad alanı boş bırakılamaz.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
                .EmailAddress().WithMessage("Lütfen e-posta adresinizi giriniz.");
            //.Must((x, y) => y != x.Customer.Email).WithMessage("Bu Email adresi zaten kayıtlı.");

        }

        private bool UniqueEmail(string email,int id)
        {
            var model = _UnitOfWork.GetRepo<Customer>().Where(x=>x.Id==id);
            //var dbEmail = _UnitOfWork.GetRepo<CustomerPicture>().Where(x => x.Customer.Email == o).SingleOrDefault();
            return true;
        }
        
    }
}
