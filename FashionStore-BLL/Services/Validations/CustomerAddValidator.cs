using System.Linq;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FluentValidation;

namespace FashionStore_BLL.Services.Validations
{
    public class CustomerAddValidator : AbstractValidator<Customer>
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerAddValidator(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim alanı boş bırakılamaz.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyad alanı boş bırakılamaz.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
                .EmailAddress().WithMessage("Lütfen e-posta adresinizi giriniz.")
                .Must(UniqueMailCheck).WithMessage("Bu Email adresi zaten kayıtlı.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre alanı boş bırakılamaz.")
                .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,16}$").WithMessage("Şifreniz en az 1 büyük harf, 1 küçük harf ve 1 sayı içermelidir. Şifreniz 6-16 karakter uzunluğunda olmalıdır.");
            RuleFor(x => x.Active).NotEmpty().WithMessage("Durum alanı boş bırakılamaz.");
            RuleFor(x => x.CustomerRoleId).NotEmpty().WithMessage("Yetki alanı boş bırakılamaz.");
        }

        private bool UniqueMailCheck(string email)
        {
            var data = _customerRepository.Where(x => x.Email == email).FirstOrDefault();

            if (data == null)
            {
                return true;
            }

            return false;
        }
    }
}
