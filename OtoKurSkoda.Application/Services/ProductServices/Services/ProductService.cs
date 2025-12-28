using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.ProductServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;
using System.Text.RegularExpressions;

namespace OtoKurSkoda.Application.Services.ProductServices.Services
{
    public class ProductService : BaseApplicationService, IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetAllAsync(ProductFilterRequest filter)
        {
            var repo = _unitOfWork.GetRepository<Product>();

            var query = repo.GetWhere(x => x.Status)
                .Include(x => x.Category)
                .Include(x => x.Manufacturer)
                .Include(x => x.Images.Where(i => i.Status))
                .AsQueryable();

            // Filters
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var search = filter.SearchTerm.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(search) ||
                                         x.Sku.ToLower().Contains(search) ||
                                         (x.OemNumber != null && x.OemNumber.ToLower().Contains(search)));
            }

            if (filter.CategoryId.HasValue)
                query = query.Where(x => x.CategoryId == filter.CategoryId);

            if (filter.ManufacturerId.HasValue)
                query = query.Where(x => x.ManufacturerId == filter.ManufacturerId);

            if (filter.MinPrice.HasValue)
                query = query.Where(x => x.Price >= filter.MinPrice);

            if (filter.MaxPrice.HasValue)
                query = query.Where(x => x.Price <= filter.MaxPrice);

            if (filter.InStock.HasValue && filter.InStock.Value)
                query = query.Where(x => x.StockQuantity > 0);

            if (filter.IsFeatured.HasValue)
                query = query.Where(x => x.IsFeatured == filter.IsFeatured);

            if (filter.IsOnSale.HasValue)
                query = query.Where(x => x.IsOnSale == filter.IsOnSale);

            // Vehicle filter
            if (filter.VehicleGenerationId.HasValue)
            {
                query = query.Where(x => x.Compatibilities.Any(c => c.VehicleGenerationId == filter.VehicleGenerationId));
            }
            else if (filter.VehicleModelId.HasValue)
            {
                query = query.Where(x => x.Compatibilities.Any(c => c.VehicleGeneration.VehicleModelId == filter.VehicleModelId));
            }
            else if (filter.BrandId.HasValue)
            {
                query = query.Where(x => x.Compatibilities.Any(c => c.VehicleGeneration.VehicleModel.BrandId == filter.BrandId));
            }

            // Sorting
            query = filter.SortBy?.ToLower() switch
            {
                "price" => filter.SortDescending ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
                "name" => filter.SortDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "date" => filter.SortDescending ? query.OrderByDescending(x => x.CreateTime) : query.OrderBy(x => x.CreateTime),
                "sold" => query.OrderByDescending(x => x.SoldCount),
                "views" => query.OrderByDescending(x => x.ViewCount),
                _ => query.OrderByDescending(x => x.CreateTime)
            };

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result = products.Select(MapToListDto).ToList();

            return SuccessPagedDataResult(result, totalCount, filter.PageNumber, filter.PageSize, "PRODUCTS_LISTED", "Ürünler listelendi.");
        }

        public async Task<ServiceResult> GetByIdAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Product>();

            var product = await repo.GetWhere(x => x.Id == id && x.Status)
                .Include(x => x.Category)
                .Include(x => x.Manufacturer)
                .Include(x => x.Images.Where(i => i.Status).OrderBy(i => i.DisplayOrder))
                .Include(x => x.Attributes).ThenInclude(a => a.AttributeDefinition)
                .Include(x => x.Compatibilities).ThenInclude(c => c.VehicleGeneration)
                    .ThenInclude(g => g.VehicleModel).ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync();

            if (product == null)
                return ErrorResult("PRODUCT_NOT_FOUND", "Ürün bulunamadı.");

            // Increment view count
            product.ViewCount++;
            repo.Update(product);
            await _unitOfWork.SaveChangesAsync();

            var result = MapToDto(product);
            return SuccessDataResult(result, "PRODUCT_FOUND", "Ürün bulundu.");
        }

        public async Task<ServiceResult> GetBySlugAsync(string slug)
        {
            var repo = _unitOfWork.GetRepository<Product>();

            var product = await repo.GetWhere(x => x.Slug == slug && x.Status)
                .Include(x => x.Category)
                .Include(x => x.Manufacturer)
                .Include(x => x.Images.Where(i => i.Status).OrderBy(i => i.DisplayOrder))
                .Include(x => x.Attributes).ThenInclude(a => a.AttributeDefinition)
                .Include(x => x.Compatibilities).ThenInclude(c => c.VehicleGeneration)
                    .ThenInclude(g => g.VehicleModel).ThenInclude(m => m.Brand)
                .FirstOrDefaultAsync();

            if (product == null)
                return ErrorResult("PRODUCT_NOT_FOUND", "Ürün bulunamadı.");

            product.ViewCount++;
            repo.Update(product);
            await _unitOfWork.SaveChangesAsync();

            var result = MapToDto(product);
            return SuccessDataResult(result, "PRODUCT_FOUND", "Ürün bulundu.");
        }

        public async Task<ServiceResult> GetByCategoryAsync(Guid categoryId, int pageNumber = 1, int pageSize = 20)
        {
            return await GetAllAsync(new ProductFilterRequest { CategoryId = categoryId, PageNumber = pageNumber, PageSize = pageSize });
        }

        public async Task<ServiceResult> GetByManufacturerAsync(Guid manufacturerId, int pageNumber = 1, int pageSize = 20)
        {
            return await GetAllAsync(new ProductFilterRequest { ManufacturerId = manufacturerId, PageNumber = pageNumber, PageSize = pageSize });
        }

        public async Task<ServiceResult> GetByVehicleAsync(Guid vehicleGenerationId, int pageNumber = 1, int pageSize = 20)
        {
            return await GetAllAsync(new ProductFilterRequest { VehicleGenerationId = vehicleGenerationId, PageNumber = pageNumber, PageSize = pageSize });
        }

        public async Task<ServiceResult> GetFeaturedAsync(int count = 10)
        {
            var repo = _unitOfWork.GetRepository<Product>();

            var products = await repo.GetWhere(x => x.Status && x.IsFeatured)
                .Include(x => x.Category)
                .Include(x => x.Manufacturer)
                .Include(x => x.Images.Where(i => i.IsMain && i.Status))
                .OrderByDescending(x => x.CreateTime)
                .Take(count)
                .ToListAsync();

            var result = products.Select(MapToCardDto).ToList();
            return SuccessListDataResult(result, "FEATURED_PRODUCTS_LISTED", "Öne çıkan ürünler listelendi.");
        }

        public async Task<ServiceResult> GetNewArrivalsAsync(int count = 10)
        {
            var repo = _unitOfWork.GetRepository<Product>();

            var products = await repo.GetWhere(x => x.Status && x.IsNewArrival)
                .Include(x => x.Manufacturer)
                .Include(x => x.Images.Where(i => i.IsMain && i.Status))
                .OrderByDescending(x => x.CreateTime)
                .Take(count)
                .ToListAsync();

            var result = products.Select(MapToCardDto).ToList();
            return SuccessListDataResult(result, "NEW_ARRIVALS_LISTED", "Yeni ürünler listelendi.");
        }

        public async Task<ServiceResult> GetBestSellersAsync(int count = 10)
        {
            var repo = _unitOfWork.GetRepository<Product>();

            var products = await repo.GetWhere(x => x.Status && x.IsBestSeller)
                .Include(x => x.Manufacturer)
                .Include(x => x.Images.Where(i => i.IsMain && i.Status))
                .OrderByDescending(x => x.SoldCount)
                .Take(count)
                .ToListAsync();

            var result = products.Select(MapToCardDto).ToList();
            return SuccessListDataResult(result, "BEST_SELLERS_LISTED", "Çok satanlar listelendi.");
        }

        public async Task<ServiceResult> GetOnSaleAsync(int count = 10)
        {
            var repo = _unitOfWork.GetRepository<Product>();

            var products = await repo.GetWhere(x => x.Status && x.IsOnSale && x.DiscountedPrice.HasValue)
                .Include(x => x.Manufacturer)
                .Include(x => x.Images.Where(i => i.IsMain && i.Status))
                .OrderByDescending(x => x.CreateTime)
                .Take(count)
                .ToListAsync();

            var result = products.Select(MapToCardDto).ToList();
            return SuccessListDataResult(result, "ON_SALE_PRODUCTS_LISTED", "İndirimdeki ürünler listelendi.");
        }

        public async Task<ServiceResult> SearchAsync(string searchTerm, int pageNumber = 1, int pageSize = 20)
        {
            return await GetAllAsync(new ProductFilterRequest { SearchTerm = searchTerm, PageNumber = pageNumber, PageSize = pageSize });
        }

        public async Task<ServiceResult> CreateAsync(CreateProductRequest request)
        {
            var repo = _unitOfWork.GetRepository<Product>();
            var categoryRepo = _unitOfWork.GetRepository<Category>();

            var category = await categoryRepo.GetByIdAsync(request.CategoryId);
            if (category == null || !category.Status)
                return ErrorResult("CATEGORY_NOT_FOUND", "Kategori bulunamadı.");

            if (await repo.AnyAsync(x => x.Sku == request.Sku && x.Status))
                return ErrorResult("SKU_EXISTS", "Bu stok kodu zaten kullanımda.");

            var slug = GenerateSlug(request.Name);
            if (await repo.AnyAsync(x => x.Slug == slug && x.Status))
                slug = $"{slug}-{DateTime.Now.Ticks}";

            var product = new Product
            {
                CategoryId = request.CategoryId,
                ManufacturerId = request.ManufacturerId,
                Name = request.Name,
                Slug = slug,
                ShortDescription = request.ShortDescription,
                Description = request.Description,
                Sku = request.Sku,
                Barcode = request.Barcode,
                OemNumber = request.OemNumber,
                Price = request.Price,
                DiscountedPrice = request.DiscountedPrice,
                CostPrice = request.CostPrice,
                TaxRate = request.TaxRate,
                StockQuantity = request.StockQuantity,
                LowStockThreshold = request.LowStockThreshold,
                TrackInventory = request.TrackInventory,
                IsFeatured = request.IsFeatured,
                IsNewArrival = request.IsNewArrival,
                IsBestSeller = request.IsBestSeller,
                IsOnSale = request.IsOnSale,
                MetaTitle = request.MetaTitle ?? request.Name,
                MetaDescription = request.MetaDescription,
                MetaKeywords = request.MetaKeywords
            };

            await repo.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return SuccessDataResult(new { product.Id, product.Slug }, "PRODUCT_CREATED", "Ürün oluşturuldu.");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateProductRequest request)
        {
            var repo = _unitOfWork.GetRepository<Product>();

            var product = await repo.GetByIdAsync(request.Id);
            if (product == null || !product.Status)
                return ErrorResult("PRODUCT_NOT_FOUND", "Ürün bulunamadı.");

            if (await repo.AnyAsync(x => x.Sku == request.Sku && x.Id != request.Id && x.Status))
                return ErrorResult("SKU_EXISTS", "Bu stok kodu zaten kullanımda.");

            var slug = GenerateSlug(request.Name);
            if (await repo.AnyAsync(x => x.Slug == slug && x.Id != request.Id && x.Status))
                slug = $"{slug}-{DateTime.Now.Ticks}";

            product.CategoryId = request.CategoryId;
            product.ManufacturerId = request.ManufacturerId;
            product.Name = request.Name;
            product.Slug = slug;
            product.ShortDescription = request.ShortDescription;
            product.Description = request.Description;
            product.Sku = request.Sku;
            product.Barcode = request.Barcode;
            product.OemNumber = request.OemNumber;
            product.Price = request.Price;
            product.DiscountedPrice = request.DiscountedPrice;
            product.CostPrice = request.CostPrice;
            product.TaxRate = request.TaxRate;
            product.StockQuantity = request.StockQuantity;
            product.LowStockThreshold = request.LowStockThreshold;
            product.TrackInventory = request.TrackInventory;
            product.IsFeatured = request.IsFeatured;
            product.IsNewArrival = request.IsNewArrival;
            product.IsBestSeller = request.IsBestSeller;
            product.IsOnSale = request.IsOnSale;
            product.MetaTitle = request.MetaTitle ?? request.Name;
            product.MetaDescription = request.MetaDescription;
            product.MetaKeywords = request.MetaKeywords;

            repo.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("PRODUCT_UPDATED", "Ürün güncellendi.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Product>();

            var product = await repo.GetByIdAsync(id);
            if (product == null || !product.Status)
                return ErrorResult("PRODUCT_NOT_FOUND", "Ürün bulunamadı.");

            product.Status = false;
            repo.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("PRODUCT_DELETED", "Ürün silindi.");
        }

        public async Task<ServiceResult> UpdateStockAsync(Guid id, int quantity)
        {
            var repo = _unitOfWork.GetRepository<Product>();

            var product = await repo.GetByIdAsync(id);
            if (product == null || !product.Status)
                return ErrorResult("PRODUCT_NOT_FOUND", "Ürün bulunamadı.");

            product.StockQuantity = quantity;
            repo.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("STOCK_UPDATED", "Stok güncellendi.");
        }

        private static string GenerateSlug(string text)
        {
            var slug = text.ToLowerInvariant();
            slug = slug.Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u")
                       .Replace("ş", "s").Replace("ö", "o").Replace("ç", "c");
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"-+", "-");
            slug = slug.Trim('-');
            return slug;
        }

        private ProductListDto MapToListDto(Product p) => new()
        {
            Id = p.Id,
            Name = p.Name,
            Slug = p.Slug,
            Sku = p.Sku,
            OemNumber = p.OemNumber,
            CategoryName = p.Category?.Name ?? "",
            ManufacturerName = p.Manufacturer?.Name,
            Price = p.Price,
            DiscountedPrice = p.DiscountedPrice,
            StockQuantity = p.StockQuantity,
            IsFeatured = p.IsFeatured,
            IsOnSale = p.IsOnSale,
            Status = p.Status,
            MainImageUrl = p.Images.FirstOrDefault(i => i.IsMain)?.Url ?? p.Images.FirstOrDefault()?.Url
        };

        private ProductCardDto MapToCardDto(Product p) => new()
        {
            Id = p.Id,
            Name = p.Name,
            Slug = p.Slug,
            Price = p.Price,
            DiscountedPrice = p.DiscountedPrice,
            MainImageUrl = p.Images.FirstOrDefault(i => i.IsMain)?.Url ?? p.Images.FirstOrDefault()?.Url,
            ManufacturerName = p.Manufacturer?.Name,
            IsOnSale = p.IsOnSale,
            IsNewArrival = p.IsNewArrival,
            StockQuantity = p.StockQuantity
        };

        private ProductDto MapToDto(Product p) => new()
        {
            Id = p.Id,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? "",
            ManufacturerId = p.ManufacturerId,
            ManufacturerName = p.Manufacturer?.Name,
            Name = p.Name,
            Slug = p.Slug,
            ShortDescription = p.ShortDescription,
            Description = p.Description,
            Sku = p.Sku,
            Barcode = p.Barcode,
            OemNumber = p.OemNumber,
            Price = p.Price,
            DiscountedPrice = p.DiscountedPrice,
            TaxRate = p.TaxRate,
            StockQuantity = p.StockQuantity,
            LowStockThreshold = p.LowStockThreshold,
            TrackInventory = p.TrackInventory,
            IsFeatured = p.IsFeatured,
            IsNewArrival = p.IsNewArrival,
            IsBestSeller = p.IsBestSeller,
            IsOnSale = p.IsOnSale,
            Status = p.Status,
            MetaTitle = p.MetaTitle,
            MetaDescription = p.MetaDescription,
            MetaKeywords = p.MetaKeywords,
            ViewCount = p.ViewCount,
            SoldCount = p.SoldCount,
            CreateTime = p.CreateTime,
            UpdateTime = p.UpdateTime,
            MainImageUrl = p.Images.FirstOrDefault(i => i.IsMain)?.Url ?? p.Images.FirstOrDefault()?.Url,
            Images = p.Images.Select(i => new ProductImageDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                Url = i.Url,
                ThumbnailUrl = i.ThumbnailUrl,
                AltText = i.AltText,
                DisplayOrder = i.DisplayOrder,
                IsMain = i.IsMain
            }).ToList(),
            Attributes = p.Attributes.Select(a => new ProductAttributeDto
            {
                Id = a.Id,
                ProductId = a.ProductId,
                AttributeDefinitionId = a.AttributeDefinitionId,
                AttributeName = a.AttributeDefinition.Name,
                Unit = a.AttributeDefinition.Unit,
                Value = a.Value
            }).ToList(),
            Compatibilities = p.Compatibilities.Select(c => new ProductCompatibilityDto
            {
                Id = c.Id,
                ProductId = c.ProductId,
                VehicleGenerationId = c.VehicleGenerationId,
                BrandName = c.VehicleGeneration.VehicleModel.Brand.Name,
                VehicleModelName = c.VehicleGeneration.VehicleModel.Name,
                VehicleGenerationName = c.VehicleGeneration.Name,
                Code = c.VehicleGeneration.Code,
                StartYear = c.VehicleGeneration.StartYear,
                EndYear = c.VehicleGeneration.EndYear,
                Notes = c.Notes
            }).ToList()
        };
    }
}