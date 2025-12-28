using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.ProductServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;

namespace OtoKurSkoda.Application.Services.ProductServices.Services
{
    public class ProductImageService : BaseApplicationService, IProductImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> GetByProductIdAsync(Guid productId)
        {
            var repo = _unitOfWork.GetRepository<ProductImage>();

            var images = await repo.GetWhere(x => x.ProductId == productId && x.Status)
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new ProductImageDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Url = x.Url,
                    ThumbnailUrl = x.ThumbnailUrl,
                    AltText = x.AltText,
                    DisplayOrder = x.DisplayOrder,
                    IsMain = x.IsMain
                })
                .ToListAsync();

            return SuccessListDataResult(images, "PRODUCT_IMAGES_LISTED", "Ürün görselleri listelendi.");
        }

        public async Task<ServiceResult> AddImageAsync(AddProductImageRequest request)
        {
            var repo = _unitOfWork.GetRepository<ProductImage>();
            var productRepo = _unitOfWork.GetRepository<Product>();

            var product = await productRepo.GetByIdAsync(request.ProductId);
            if (product == null || !product.Status)
                return ErrorResult("PRODUCT_NOT_FOUND", "Ürün bulunamadı.");

            // İlk görsel ise otomatik main yap
            var hasImages = await repo.AnyAsync(x => x.ProductId == request.ProductId && x.Status);

            var image = new ProductImage
            {
                ProductId = request.ProductId,
                Url = request.Url,
                ThumbnailUrl = request.ThumbnailUrl,
                AltText = request.AltText,
                DisplayOrder = request.DisplayOrder,
                IsMain = !hasImages || request.IsMain
            };

            // Eğer bu main olacaksa diğerlerinin main'ini kaldır
            if (image.IsMain)
            {
                var existingImages = await repo.GetWhere(x => x.ProductId == request.ProductId && x.IsMain && x.Status).ToListAsync();
                foreach (var img in existingImages)
                {
                    img.IsMain = false;
                    repo.Update(img);
                }
            }

            await repo.AddAsync(image);
            await _unitOfWork.SaveChangesAsync();

            return SuccessDataResult(new ProductImageDto
            {
                Id = image.Id,
                ProductId = image.ProductId,
                Url = image.Url,
                ThumbnailUrl = image.ThumbnailUrl,
                AltText = image.AltText,
                DisplayOrder = image.DisplayOrder,
                IsMain = image.IsMain
            }, "PRODUCT_IMAGE_ADDED", "Görsel eklendi.");
        }

        public async Task<ServiceResult> UpdateImageAsync(UpdateProductImageRequest request)
        {
            var repo = _unitOfWork.GetRepository<ProductImage>();

            var image = await repo.GetByIdAsync(request.Id);
            if (image == null || !image.Status)
                return ErrorResult("IMAGE_NOT_FOUND", "Görsel bulunamadı.");

            image.Url = request.Url;
            image.ThumbnailUrl = request.ThumbnailUrl;
            image.AltText = request.AltText;
            image.DisplayOrder = request.DisplayOrder;

            if (request.IsMain && !image.IsMain)
            {
                var existingImages = await repo.GetWhere(x => x.ProductId == image.ProductId && x.IsMain && x.Status).ToListAsync();
                foreach (var img in existingImages)
                {
                    img.IsMain = false;
                    repo.Update(img);
                }
                image.IsMain = true;
            }

            repo.Update(image);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("PRODUCT_IMAGE_UPDATED", "Görsel güncellendi.");
        }

        public async Task<ServiceResult> DeleteImageAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<ProductImage>();

            var image = await repo.GetByIdAsync(id);
            if (image == null || !image.Status)
                return ErrorResult("IMAGE_NOT_FOUND", "Görsel bulunamadı.");

            var wasMain = image.IsMain;
            var productId = image.ProductId;

            image.Status = false;
            repo.Update(image);
            await _unitOfWork.SaveChangesAsync();

            // Silinen main ise ilk görseli main yap
            if (wasMain)
            {
                var firstImage = await repo.GetWhere(x => x.ProductId == productId && x.Status)
                    .OrderBy(x => x.DisplayOrder)
                    .FirstOrDefaultAsync();

                if (firstImage != null)
                {
                    firstImage.IsMain = true;
                    repo.Update(firstImage);
                    await _unitOfWork.SaveChangesAsync();
                }
            }

            return SuccessResult("PRODUCT_IMAGE_DELETED", "Görsel silindi.");
        }

        public async Task<ServiceResult> SetMainImageAsync(Guid productId, Guid imageId)
        {
            var repo = _unitOfWork.GetRepository<ProductImage>();

            var image = await repo.GetByIdAsync(imageId);
            if (image == null || !image.Status || image.ProductId != productId)
                return ErrorResult("IMAGE_NOT_FOUND", "Görsel bulunamadı.");

            // Tüm görsellerin main'ini kaldır
            var allImages = await repo.GetWhere(x => x.ProductId == productId && x.Status).ToListAsync();
            foreach (var img in allImages)
            {
                img.IsMain = img.Id == imageId;
                repo.Update(img);
            }

            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("MAIN_IMAGE_SET", "Ana görsel ayarlandı.");
        }

        public async Task<ServiceResult> ReorderImagesAsync(Guid productId, List<Guid> imageIds)
        {
            var repo = _unitOfWork.GetRepository<ProductImage>();

            var images = await repo.GetWhere(x => x.ProductId == productId && x.Status).ToListAsync();

            for (int i = 0; i < imageIds.Count; i++)
            {
                var image = images.FirstOrDefault(x => x.Id == imageIds[i]);
                if (image != null)
                {
                    image.DisplayOrder = i;
                    repo.Update(image);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("IMAGES_REORDERED", "Görsel sıralaması güncellendi.");
        }
    }
}