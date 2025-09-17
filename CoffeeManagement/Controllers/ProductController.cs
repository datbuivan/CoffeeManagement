using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController<Product, ProductDto>
    {
        public ProductController(IGenericRepository<Product> repo, IMapper mapper)
            : base(repo, mapper)
        {
        }
    }
}
