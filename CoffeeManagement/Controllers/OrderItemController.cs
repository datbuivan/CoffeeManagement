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
    public class OrderItemController : BaseController<OrderItem, OrderItemDto>
    {
        public OrderItemController(IGenericRepository<OrderItem> repo, IMapper mapper)
            : base(repo, mapper) { }
    }
}
