using AutoMapper;
using CoffeeManagement.Data.Dtos.ProductSize;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeManagement.Services
{
    public class ProductSizeService : IProductSizeService
    {
        private readonly IGenericRepository<ProductSize> _repo;
        private readonly IMapper _mapper;

        public ProductSizeService(IGenericRepository<ProductSize> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductSizeResultDto>> GetByProductIdAsync(Guid productId)
        {
            var sizes = await _repo.FindAllAsync(x => x.ProductId == productId);

            return _mapper.Map<IEnumerable<ProductSizeResultDto>>(sizes);
        }
    }
}
