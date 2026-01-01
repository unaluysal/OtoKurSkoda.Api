using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;

namespace OtoKurSkoda.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

        public UploadController(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        /// <summary>
        /// Ürün görseli yükle (opsiyonel boyutlandırma ile)
        /// </summary>
        [HttpPost("product-image")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> UploadProductImage(
            IFormFile file,
            [FromQuery] int? width,
            [FromQuery] int? height,
            [FromQuery] bool maintainAspectRatio = true,
            [FromQuery] string format = "webp")
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Ok(new { Status = false, Message = "Dosya seçilmedi", MessageCode = "NO_FILE" });

                if (file.Length > MaxFileSize)
                    return Ok(new { Status = false, Message = "Dosya boyutu çok büyük (max 10MB)", MessageCode = "FILE_TOO_LARGE" });

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(extension))
                    return Ok(new { Status = false, Message = "Desteklenmeyen dosya formatı", MessageCode = "INVALID_FORMAT" });

                // Upload klasörünü oluştur - ContentRootPath kullanıyoruz (wwwroot değil)
                var uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads", "products");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Thumbnail klasörü
                var thumbnailsFolder = Path.Combine(uploadsFolder, "thumbnails");
                if (!Directory.Exists(thumbnailsFolder))
                    Directory.CreateDirectory(thumbnailsFolder);

                // Unique dosya adı oluştur
                var fileName = $"{Guid.NewGuid()}{GetExtensionForFormat(format)}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                var thumbnailPath = Path.Combine(thumbnailsFolder, fileName);

                using (var stream = file.OpenReadStream())
                using (var image = await Image.LoadAsync(stream))
                {
                    // Ana görsel - boyutlandırma
                    if (width.HasValue || height.HasValue)
                    {
                        var resizeOptions = new ResizeOptions
                        {
                            Mode = maintainAspectRatio ? ResizeMode.Max : ResizeMode.Stretch,
                            Size = new Size(width ?? 0, height ?? 0)
                        };

                        if (width.HasValue && !height.HasValue)
                            resizeOptions.Size = new Size(width.Value, 0);
                        else if (!width.HasValue && height.HasValue)
                            resizeOptions.Size = new Size(0, height.Value);

                        image.Mutate(x => x.Resize(resizeOptions));
                    }

                    // Ana görseli kaydet
                    await SaveImageAsync(image, filePath, format);

                    // Thumbnail oluştur (max 300px)
                    using (var thumbnailImage = image.Clone(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(300, 300)
                    })))
                    {
                        await SaveImageAsync(thumbnailImage, thumbnailPath, format);
                    }
                }

                // URL'leri oluştur
                var baseUrl = _configuration.GetSection("AppSettings:BaseUrl").Value ?? $"{Request.Scheme}://{Request.Host}";
                var imageUrl = $"{baseUrl}/uploads/products/{fileName}";
                var thumbnailUrl = $"{baseUrl}/uploads/products/thumbnails/{fileName}";

                return Ok(new
                {
                    Status = true,
                    Message = "Görsel başarıyla yüklendi",
                    MessageCode = "IMAGE_UPLOADED",
                    Data = new
                    {
                        Url = imageUrl,
                        ThumbnailUrl = thumbnailUrl,
                        FileName = fileName
                    }
                });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = $"Görsel yüklenirken hata: {ex.Message}", MessageCode = "UPLOAD_ERROR" });
            }
        }

        /// <summary>
        /// Kırpılmış görsel yükle (crop koordinatları ile)
        /// </summary>
        [HttpPost("product-image-cropped")]
        [Authorize(Roles = "Product_Update")]
        public async Task<IActionResult> UploadCroppedProductImage(
            IFormFile file,
            [FromQuery] int cropX,
            [FromQuery] int cropY,
            [FromQuery] int cropWidth,
            [FromQuery] int cropHeight,
            [FromQuery] int? finalWidth,
            [FromQuery] int? finalHeight,
            [FromQuery] string format = "webp")
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Ok(new { Status = false, Message = "Dosya seçilmedi", MessageCode = "NO_FILE" });

                if (file.Length > MaxFileSize)
                    return Ok(new { Status = false, Message = "Dosya boyutu çok büyük (max 10MB)", MessageCode = "FILE_TOO_LARGE" });

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(extension))
                    return Ok(new { Status = false, Message = "Desteklenmeyen dosya formatı", MessageCode = "INVALID_FORMAT" });

                var uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads", "products");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var thumbnailsFolder = Path.Combine(uploadsFolder, "thumbnails");
                if (!Directory.Exists(thumbnailsFolder))
                    Directory.CreateDirectory(thumbnailsFolder);

                var fileName = $"{Guid.NewGuid()}{GetExtensionForFormat(format)}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                var thumbnailPath = Path.Combine(thumbnailsFolder, fileName);

                using (var stream = file.OpenReadStream())
                using (var image = await Image.LoadAsync(stream))
                {
                    // Kırpma işlemi
                    var cropRect = new Rectangle(cropX, cropY, cropWidth, cropHeight);
                    image.Mutate(x => x.Crop(cropRect));

                    // Final boyutlandırma (opsiyonel)
                    if (finalWidth.HasValue || finalHeight.HasValue)
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = new Size(finalWidth ?? 0, finalHeight ?? 0)
                        }));
                    }

                    // Ana görseli kaydet
                    await SaveImageAsync(image, filePath, format);

                    // Thumbnail oluştur
                    using (var thumbnailImage = image.Clone(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(300, 300)
                    })))
                    {
                        await SaveImageAsync(thumbnailImage, thumbnailPath, format);
                    }
                }

                var baseUrl = _configuration.GetSection("AppSettings:BaseUrl").Value ?? $"{Request.Scheme}://{Request.Host}";
                var imageUrl = $"{baseUrl}/uploads/products/{fileName}";
                var thumbnailUrl = $"{baseUrl}/uploads/products/thumbnails/{fileName}";

                return Ok(new
                {
                    Status = true,
                    Message = "Görsel başarıyla yüklendi",
                    MessageCode = "IMAGE_UPLOADED",
                    Data = new
                    {
                        Url = imageUrl,
                        ThumbnailUrl = thumbnailUrl,
                        FileName = fileName
                    }
                });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = $"Görsel yüklenirken hata: {ex.Message}", MessageCode = "UPLOAD_ERROR" });
            }
        }

        /// <summary>
        /// Görsel sil
        /// </summary>
        [HttpPost("delete")]
        [Authorize(Roles = "Product_Update")]
        public IActionResult DeleteImage([FromBody] string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return Ok(new { Status = false, Message = "Dosya adı belirtilmedi", MessageCode = "NO_FILENAME" });

                // Güvenlik: sadece dosya adını al, path traversal engelle
                fileName = Path.GetFileName(fileName);

                var uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads", "products");
                var filePath = Path.Combine(uploadsFolder, fileName);
                var thumbnailPath = Path.Combine(uploadsFolder, "thumbnails", fileName);

                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                if (System.IO.File.Exists(thumbnailPath))
                    System.IO.File.Delete(thumbnailPath);

                return Ok(new { Status = true, Message = "Görsel silindi", MessageCode = "IMAGE_DELETED" });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Message = $"Görsel silinirken hata: {ex.Message}", MessageCode = "DELETE_ERROR" });
            }
        }

        private static string GetExtensionForFormat(string format)
        {
            return format.ToLower() switch
            {
                "webp" => ".webp",
                "png" => ".png",
                "jpg" or "jpeg" => ".jpg",
                _ => ".webp"
            };
        }

        private static async Task SaveImageAsync(Image image, string path, string format)
        {
            switch (format.ToLower())
            {
                case "webp":
                    await image.SaveAsync(path, new WebpEncoder { Quality = 85 });
                    break;
                case "png":
                    await image.SaveAsync(path, new PngEncoder { CompressionLevel = PngCompressionLevel.BestCompression });
                    break;
                case "jpg":
                case "jpeg":
                    await image.SaveAsync(path, new JpegEncoder { Quality = 85 });
                    break;
                default:
                    await image.SaveAsync(path, new WebpEncoder { Quality = 85 });
                    break;
            }
        }
    }
}
