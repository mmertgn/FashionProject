using System.Linq;
using FashionStore.Entity.Entities;
using FashionStore.Repository.Repositories.Abstracts;
using FluentValidation;

namespace FashionStore_BLL.Validations.ProductValidations
{
    public class ProductUpdateValidator : AbstractValidator<Product>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductUpdateValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Ürün Adı alanı boş geçilemez.")
                .Must(UniqueNameCheck).WithMessage("Bu isimde bir ürün zaten kayıtlı.");
            RuleFor(x => x.ShortDescription)
                .NotNull().WithMessage("Kısa Açıklama alanı boş geçilemez.");
            RuleFor(x => x.CategoryId)
                .NotNull().WithMessage("Alt Kategori alanı boş geçilemez.");
            RuleFor(x => x.FullDescription)
                .NotNull().WithMessage("Uzun Açıklama alanı boş geçilemez.");
            RuleFor(x => x.StockQuantity)
                .NotNull().WithMessage("Stok Miktarı alanı boş geçilemez.")
                .GreaterThanOrEqualTo(1).WithMessage("Stok Miktarı 1'den küçük olamaz.");
            RuleFor(x => x.OrderMinimumQuantity)
                .NotNull().WithMessage("Minimum Sipariş Miktarı alanı boş geçilemez.")
                .GreaterThanOrEqualTo(1).WithMessage("Minimum Sipariş Miktarı 1'den küçük olamaz.");
            RuleFor(x => x.OrderMaximumQuantit)
                .NotNull().WithMessage("Maximum Sipariş Miktarı alanı boş geçilemez.")
                .LessThanOrEqualTo(100).WithMessage("Maximum Sipariş Miktarı 100'den büyük olamaz.");
            RuleFor(x => x.Price)
                .NotNull().WithMessage("Fiyat alanı boş geçilemez.")
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.");
            RuleFor(x => x.DisplayOrder)
                .NotNull().WithMessage("Görüntülenme Sırası alanı boş geçilemez.")
                .GreaterThanOrEqualTo(1).WithMessage("Görüntülenme Sırası 1'den küçük olamaz.");
            RuleFor(x => x.MarkAsNewEndTime)
                .GreaterThan(x => x.MarkAsNewStartTime)
                .WithMessage("Bitiş tarihi, başlagıç tarihinden sonra olmak zorundadır.");
        }
        private bool UniqueNameCheck(Product product, string name)
        {
            var data = _unitOfWork.GetRepo<Product>().Where(x => x.Name == name && x.Id != product.Id && x.Deleted == false).FirstOrDefault();

            if (data == null)
            {
                return true;
            }

            return false;
        }
    }
}
