using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.Product;
using CoffeeManagement.Data.Entities;

namespace CoffeeManagement.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResultDto>> Get();
        Task<ProductResultDto> GetByKey(Guid id);
        Task<ProductResultDto> Add(ProductCreateDto dto, IFormFile? file);
        Task<ProductResultDto> Update(Guid id, ProductUpdateDto dto, IFormFile? file);
        Task Delete(Guid id);
    }
}
