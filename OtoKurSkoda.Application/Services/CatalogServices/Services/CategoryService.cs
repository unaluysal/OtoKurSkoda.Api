using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtoKurSkoda.Application.Defaults;
using OtoKurSkoda.Application.Dtos;
using OtoKurSkoda.Application.Services.CatalogServices.Interfaces;
using OtoKurSkoda.Domain.Entitys;
using OtoKurSkoda.Infrastructure.UnitOfWork;
using System.Text.RegularExpressions;

namespace OtoKurSkoda.Application.Services.CatalogServices.Services
{
    public class CategoryService : BaseApplicationService, ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            var repo = _unitOfWork.GetRepository<Category>();

            var categories = await repo.GetWhere(x => x.Status)
                .Include(x => x.Parent)
                .Include(x => x.Children.Where(c => c.Status))
                .Include(x => x.Products.Where(p => p.Status))
                .OrderBy(x => x.Level)
                .ThenBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();

            var result = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                ParentId = c.ParentId,
                ParentName = c.Parent?.Name,
                Name = c.Name,
                Slug = c.Slug,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                IconClass = c.IconClass,
                Color = c.Color,
                DisplayOrder = c.DisplayOrder,
                Level = c.Level,
                MetaTitle = c.MetaTitle,
                MetaDescription = c.MetaDescription,
                MetaKeywords = c.MetaKeywords,
                Status = c.Status,
                ChildCount = c.Children.Count,
                ProductCount = c.Products.Count
            }).ToList();

            return SuccessListDataResult(result, "CATEGORIES_LISTED", "Kategoriler listelendi.");
        }

        public async Task<ServiceResult> GetTreeAsync()
        {
            var repo = _unitOfWork.GetRepository<Category>();

            var allCategories = await repo.GetWhere(x => x.Status)
                .Include(x => x.Products.Where(p => p.Status))
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();

            var rootCategories = allCategories.Where(c => c.ParentId == null).ToList();

            var result = rootCategories.Select(c => BuildCategoryTree(c, allCategories)).ToList();

            return SuccessListDataResult(result, "CATEGORY_TREE_LISTED", "Kategori ağacı listelendi.");
        }

        private CategoryTreeDto BuildCategoryTree(Category category, List<Category> allCategories)
        {
            var children = allCategories.Where(c => c.ParentId == category.Id).ToList();

            return new CategoryTreeDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                IconClass = category.IconClass,
                Color = category.Color,
                DisplayOrder = category.DisplayOrder,
                Level = category.Level,
                ProductCount = category.Products.Count,
                Children = children.Select(c => BuildCategoryTree(c, allCategories)).ToList()
            };
        }

        public async Task<ServiceResult> GetRootCategoriesAsync()
        {
            var repo = _unitOfWork.GetRepository<Category>();

            var categories = await repo.GetWhere(x => x.ParentId == null && x.Status)
                .Include(x => x.Children.Where(c => c.Status))
                .Include(x => x.Products.Where(p => p.Status))
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();

            var result = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                ParentId = c.ParentId,
                Name = c.Name,
                Slug = c.Slug,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                IconClass = c.IconClass,
                Color = c.Color,
                DisplayOrder = c.DisplayOrder,
                Level = c.Level,
                Status = c.Status,
                ChildCount = c.Children.Count,
                ProductCount = c.Products.Count
            }).ToList();

            return SuccessListDataResult(result, "ROOT_CATEGORIES_LISTED", "Ana kategoriler listelendi.");
        }

        public async Task<ServiceResult> GetChildrenAsync(Guid parentId)
        {
            var repo = _unitOfWork.GetRepository<Category>();

            var categories = await repo.GetWhere(x => x.ParentId == parentId && x.Status)
                .Include(x => x.Children.Where(c => c.Status))
                .Include(x => x.Products.Where(p => p.Status))
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name)
                .ToListAsync();

            var result = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                ParentId = c.ParentId,
                Name = c.Name,
                Slug = c.Slug,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                IconClass = c.IconClass,
                Color = c.Color,
                DisplayOrder = c.DisplayOrder,
                Level = c.Level,
                Status = c.Status,
                ChildCount = c.Children.Count,
                ProductCount = c.Products.Count
            }).ToList();

            return SuccessListDataResult(result, "CHILD_CATEGORIES_LISTED", "Alt kategoriler listelendi.");
        }

        public async Task<ServiceResult> GetByIdAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Category>();

            var category = await repo.GetWhere(x => x.Id == id && x.Status)
                .Include(x => x.Parent)
                .Include(x => x.Children.Where(c => c.Status))
                .Include(x => x.Products.Where(p => p.Status))
                .FirstOrDefaultAsync();

            if (category == null)
                return ErrorResult("CATEGORY_NOT_FOUND", "Kategori bulunamadı.");

            var result = new CategoryDto
            {
                Id = category.Id,
                ParentId = category.ParentId,
                ParentName = category.Parent?.Name,
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                IconClass = category.IconClass,
                Color = category.Color,
                DisplayOrder = category.DisplayOrder,
                Level = category.Level,
                MetaTitle = category.MetaTitle,
                MetaDescription = category.MetaDescription,
                MetaKeywords = category.MetaKeywords,
                Status = category.Status,
                ChildCount = category.Children.Count,
                ProductCount = category.Products.Count
            };

            return SuccessDataResult(result, "CATEGORY_FOUND", "Kategori bulundu.");
        }

        public async Task<ServiceResult> GetBySlugAsync(string slug)
        {
            var repo = _unitOfWork.GetRepository<Category>();

            var category = await repo.GetWhere(x => x.Slug == slug && x.Status)
                .Include(x => x.Parent)
                .Include(x => x.Children.Where(c => c.Status))
                .Include(x => x.Products.Where(p => p.Status))
                .FirstOrDefaultAsync();

            if (category == null)
                return ErrorResult("CATEGORY_NOT_FOUND", "Kategori bulunamadı.");

            var result = new CategoryDto
            {
                Id = category.Id,
                ParentId = category.ParentId,
                ParentName = category.Parent?.Name,
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                IconClass = category.IconClass,
                Color = category.Color,
                DisplayOrder = category.DisplayOrder,
                Level = category.Level,
                MetaTitle = category.MetaTitle,
                MetaDescription = category.MetaDescription,
                MetaKeywords = category.MetaKeywords,
                Status = category.Status,
                ChildCount = category.Children.Count,
                ProductCount = category.Products.Count
            };

            return SuccessDataResult(result, "CATEGORY_FOUND", "Kategori bulundu.");
        }

        public async Task<ServiceResult> CreateAsync(CreateCategoryRequest request)
        {
            var repo = _unitOfWork.GetRepository<Category>();

            var slug = GenerateSlug(request.Name);
            int level = 0;

            if (request.ParentId.HasValue)
            {
                var parent = await repo.GetByIdAsync(request.ParentId.Value);
                if (parent == null || !parent.Status)
                    return ErrorResult("PARENT_CATEGORY_NOT_FOUND", "Üst kategori bulunamadı.");
                level = parent.Level + 1;
            }

            if (await repo.AnyAsync(x => x.Slug == slug && x.Status))
            {
                slug = $"{slug}-{DateTime.Now.Ticks}";
            }

            var category = new Category
            {
                ParentId = request.ParentId,
                Name = request.Name,
                Slug = slug,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                IconClass = request.IconClass,
                Color = request.Color,
                DisplayOrder = request.DisplayOrder,
                Level = level,
                MetaTitle = request.MetaTitle ?? request.Name,
                MetaDescription = request.MetaDescription,
                MetaKeywords = request.MetaKeywords
            };

            await repo.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return SuccessDataResult(new CategoryDto
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                IconClass = category.IconClass,
                Color = category.Color,
                DisplayOrder = category.DisplayOrder,
                Level = category.Level,
                MetaTitle = category.MetaTitle,
                MetaDescription = category.MetaDescription,
                MetaKeywords = category.MetaKeywords,
                Status = category.Status
            }, "CATEGORY_CREATED", "Kategori oluşturuldu.");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateCategoryRequest request)
        {
            var repo = _unitOfWork.GetRepository<Category>();

            var category = await repo.GetByIdAsync(request.Id);

            if (category == null || !category.Status)
                return ErrorResult("CATEGORY_NOT_FOUND", "Kategori bulunamadı.");

            if (request.ParentId == request.Id)
                return ErrorResult("INVALID_PARENT", "Kategori kendisinin üst kategorisi olamaz.");

            var slug = GenerateSlug(request.Name);
            int level = 0;

            if (request.ParentId.HasValue)
            {
                var parent = await repo.GetByIdAsync(request.ParentId.Value);
                if (parent == null || !parent.Status)
                    return ErrorResult("PARENT_CATEGORY_NOT_FOUND", "Üst kategori bulunamadı.");
                level = parent.Level + 1;
            }

            if (await repo.AnyAsync(x => x.Slug == slug && x.Id != request.Id && x.Status))
            {
                slug = $"{slug}-{DateTime.Now.Ticks}";
            }

            category.ParentId = request.ParentId;
            category.Name = request.Name;
            category.Slug = slug;
            category.Description = request.Description;
            category.ImageUrl = request.ImageUrl;
            category.IconClass = request.IconClass;
            category.Color = request.Color;
            category.DisplayOrder = request.DisplayOrder;
            category.Level = level;
            category.MetaTitle = request.MetaTitle ?? request.Name;
            category.MetaDescription = request.MetaDescription;
            category.MetaKeywords = request.MetaKeywords;

            repo.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return SuccessDataResult(new CategoryDto
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                IconClass = category.IconClass,
                Color = category.Color,
                DisplayOrder = category.DisplayOrder,
                Level = category.Level,
                MetaTitle = category.MetaTitle,
                MetaDescription = category.MetaDescription,
                MetaKeywords = category.MetaKeywords,
                Status = category.Status
            }, "CATEGORY_UPDATED", "Kategori güncellendi.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Category>();

            var category = await repo.GetWhere(x => x.Id == id && x.Status)
                .Include(x => x.Children.Where(c => c.Status))
                .Include(x => x.Products.Where(p => p.Status))
                .FirstOrDefaultAsync();

            if (category == null)
                return ErrorResult("CATEGORY_NOT_FOUND", "Kategori bulunamadı.");

            if (category.Children.Any())
                return ErrorResult("CATEGORY_HAS_CHILDREN", "Bu kategorinin alt kategorileri var. Önce alt kategorileri silin.");

            if (category.Products.Any())
                return ErrorResult("CATEGORY_HAS_PRODUCTS", "Bu kategoride ürünler var. Önce ürünleri başka kategoriye taşıyın.");

            category.Status = false;
            repo.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return SuccessResult("CATEGORY_DELETED", "Kategori silindi.");
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
    }
}
