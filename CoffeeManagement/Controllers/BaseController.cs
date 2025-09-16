using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<T> : ControllerBase
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly IBaseService<T> Service;
        protected readonly ILogger<BaseController<T>> Log;

        protected BaseController(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Service = ServiceProvider.GetRequiredService<IBaseService<T>>();
            Log = ServiceProvider.GetRequiredService<ILogger<BaseController<T>>>();
        }

        [HttpGet]
        public virtual IActionResult Get() => Ok(Service.Get(Request.QueryString).Result);

        [HttpGet("{key}")]
        public virtual IActionResult Get(string key) => Ok(Service.Get(key).Result);

        [HttpPost]
        public virtual IActionResult Post([FromBody] T f) => Ok(Service.Add(f).Result);

        [HttpPost("AddRange")]
        public IActionResult AddRange([FromBody] IEnumerable<T> models) => Ok(Service.AddRange(models));

        [HttpPost("UpdateRange")]
        public IActionResult UpdateRange([FromBody] IEnumerable<T> models) => Ok(Service.UpdateRange(models));

        [HttpPut]
        public virtual IActionResult Put([FromBody] T f) => Ok(Service.Update(f).Result);

        [HttpDelete("{key}")]
        public virtual IActionResult Delete(string key)
        {
            Service.Delete(key).Wait();
            return Ok();
        }
    }
}
