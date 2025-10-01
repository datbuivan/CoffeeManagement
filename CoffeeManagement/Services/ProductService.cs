using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CoffeeManagement.Data.Dtos.Product;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Http;

namespace CoffeeManagement.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _repo;
        private readonly IUnitOfWork _uow;
        private readonly ICloudinaryService _cloud;

        public ProductService(IGenericRepository<Product> repo, IUnitOfWork uow, ICloudinaryService cloud)
        {
            _repo = repo;
            _uow = uow;
            _cloud = cloud;
        }

        public async Task<IEnumerable<ProductResultDto>> Get()
        {
            var products = await _repo.ListAllAsync();
            return products.Select(p => new ProductResultDto
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                ImageUrl = p.ImageUrl,
                IsAvailable = p.IsAvailable
            });
        }

        public async Task<ProductResultDto> GetByKey(Guid id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with Id {id} not found.");

            return new ProductResultDto
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl,
                IsAvailable = product.IsAvailable
            };
        }

        public async Task<ProductResultDto> Add(ProductCreateDto dto, IFormFile? file)
        {
            await using var transaction = await _uow.BeginTransactionAsync();
            try
            {
                var product = new Product
                {
                    Name = dto.Name,
                    CategoryId = dto.CategoryId,
                    IsAvailable = dto.IsAvailable
                };

                if (file != null)
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, file.OpenReadStream()),
                        Folder = "coffee/products"
                    };
                    var uploadResult = await _cloud.UploadAsync(uploadParams);
                    product.ImageUrl = uploadResult.SecureUrl.ToString();
                }

                _repo.Add(product);
                await _uow.Complete();
                await transaction.CommitAsync();

                return new ProductResultDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    CategoryId = product.CategoryId,
                    IsAvailable = product.IsAvailable,
                    ImageUrl = product.ImageUrl
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("An error occurred while creating the product.", ex);
            }
        }

        public async Task<ProductResultDto> Update(Guid id, ProductUpdateDto dto, IFormFile? file)
        {
            await using var transaction = await _uow.BeginTransactionAsync();
            try
            {
                var product = await _repo.GetByIdAsync(id);
                if (product == null)
                    throw new KeyNotFoundException($"Product with Id {id} not found.");

                product.Name = dto.Name;
                product.CategoryId = dto.CategoryId;
                product.IsAvailable = dto.IsAvailable;

                if (file != null)
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, file.OpenReadStream()),
                        Folder = "coffee/products"
                    };
                    var uploadResult = await _cloud.UploadAsync(uploadParams);
                    product.ImageUrl = uploadResult.SecureUrl.ToString();
                }

                _repo.Update(product);
                await _uow.Complete();
                await transaction.CommitAsync();

                return new ProductResultDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    CategoryId = product.CategoryId,
                    IsAvailable = product.IsAvailable,
                    ImageUrl = product.ImageUrl
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("An error occurred while updating the product.", ex);
            }
        }

        public async Task Delete(Guid id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with Id {id} not found.");

            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var publicId = GetPublicIdFromUrl(product.ImageUrl);
                if (!string.IsNullOrEmpty(publicId))
                {
                    var delParams = new DeletionParams(publicId);
                    await _cloud.DestroyAsync(delParams);
                }
            }

            _repo.Remove(product);
            await _uow.Complete();
        }

        private string? GetPublicIdFromUrl(string imageUrl)
        {
            try
            {
                var uri = new Uri(imageUrl);
                var segments = uri.AbsolutePath.Split('/');
                var folderAndFile = string.Join("/", segments.Skip(segments.ToList().IndexOf("upload") + 2));
                return Path.Combine(Path.GetDirectoryName(folderAndFile) ?? "", Path.GetFileNameWithoutExtension(folderAndFile))
                           .Replace("\\", "/");
            }
            catch
            {
                return null;
            }
        }
    }
}
