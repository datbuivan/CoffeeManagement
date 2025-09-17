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
    public class ReportInventorySummaryController : BaseController<ReportInventorySummary, ReportInventorySummaryDto>
    {
        public ReportInventorySummaryController(IGenericRepository<ReportInventorySummary> repo, IMapper mapper)
            : base(repo, mapper) { }
    }
}
