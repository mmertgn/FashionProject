using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FluentValidation;

namespace FashionStore_BLL.Validations.CustomerValidations
{
    public class SiteCustomerRegisterValidator : AbstractValidator<Customer>
    {
        private readonly IUnitOfWork _unitOfWork;
        public SiteCustomerRegisterValidator(IUnitOfWork unitOfWork,string PasswordConfirm)
        {
            _unitOfWork = unitOfWork;
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim alanı boş bırakılamaz.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Soyad alanı boş bırakılamaz.");
            RuleFor(x => x.DateOfBirth)
                .Must(BeAValidDate).WithMessage("Doğum Tarihi alanı boş bırakılamaz.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email alanı boş bırakılamaz.")
                .EmailAddress().WithMessage("Lütfen e-posta adresinizi giriniz.")
                .Must(UniqueMailCheck).WithMessage("Bu Email adresi zaten kayıtlı.");
            RuleFor(x => x.Password)
                .Equal(PasswordConfirm).WithMessage("Şifreler uyuşmamaktadır.")
                .NotEmpty().WithMessage("Şifre alanı boş bırakılamaz.")
                .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,16}$").WithMessage("Şifreniz en az 1 büyük harf, 1 küçük harf ve 1 sayı içermelidir. Şifreniz 6-16 karakter uzunluğunda olmalıdır.");
        }

        private bool BeAValidDate(DateTime date)
        {
            if (date == default(DateTime))
                return false;
            return true;
        }

        private bool UniqueMailCheck(string email)
        {
            var data = _unitOfWork.GetRepo<Customer>().Where(x => x.Email == email && x.Deleted == false).FirstOrDefault();

            if (data == null)
            {
                return true;
            }

            return false;
        }
    }
}
