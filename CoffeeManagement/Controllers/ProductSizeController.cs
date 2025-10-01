using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.ProductSize;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSizeController
        : BaseController<ProductSize, ProductSizeCreateDto, ProductSizeUpdateDto, ProductSizeResultDto>
    {
        public ProductSizeController(IGenericRepository<ProductSize> repo, IMapper mapper)
            : base(repo, mapper)
        {
        }
    }
}

