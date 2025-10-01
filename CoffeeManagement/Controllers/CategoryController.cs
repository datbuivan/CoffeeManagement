using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.Category;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController
        : BaseController<Category, CategoryCreateDto, CategoryUpdateDto, CategoryResultDto>
    {
        public CategoryController(IGenericRepository<Category> repo, IMapper mapper)
            : base(repo, mapper)
        {
        }
    }
}
