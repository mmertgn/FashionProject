using FashionStore.Entity.Entities;
using FluentValidation;

namespace FashionStore_BLL.Services.Validations
{
    public class ProfilUpdateValidator : AbstractValidator<CustomerPicture>
    {
        //public IUnitOfWork _UnitOfWork;
        public ProfilUpdateValidator()
        {
            RuleFor(x => x.Customer.Name).NotEmpty().WithMessage("İsim alanı boş bırakılamaz.");
            RuleFor(x => x.Customer.Surname).NotEmpty().WithMessage("Soyad alanı boş bırakılamaz.");
            RuleFor(x => x.Customer.Email)
                .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
                .EmailAddress().WithMessage("Lütfen e-posta adresinizi giriniz.")
                .Must((x, y) => y != x.Customer.Email).WithMessage("Bu Email adresi zaten kayıtlı.");
            RuleFor(x => x.Customer.Password)
                .Length(6, 16).WithMessage("Şifreniz 6-16 karakter uzunluğunda olmalıdır.")
                .NotEmpty().WithMessage("\"Şifre\" alanını boş bıraktınız."); ;
            
        }

        private bool UniqueEmail(string o)
        {
            //var dbEmail = _UnitOfWork.GetRepo<CustomerPicture>().Where(x => x.Customer.Email == o).SingleOrDefault();
            return true;
        }
        
    }
}
