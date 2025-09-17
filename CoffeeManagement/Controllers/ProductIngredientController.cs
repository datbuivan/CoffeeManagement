using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductIngredientController : BaseController<ProductIngredient, ProductIngredientDto>
    {
        public ProductIngredientController(IGenericRepository<ProductIngredient> repo, IMapper mapper)
            : base(repo, mapper) { }
    }
}
