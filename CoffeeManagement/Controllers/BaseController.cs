using AutoMapper;
using CoffeeManagement.Data.Entities.Custom;
using CoffeeManagement.Errors;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TEntity, TCreateDto, TUpdateDto, TResultDto> : ControllerBase
        where TEntity : BaseEntities, new()
        where TCreateDto : class
        where TUpdateDto : class
        where TResultDto : class
    {
        protected readonly IGenericRepository<TEntity> _repo;
        protected readonly IMapper _mapper;

        public BaseController(IGenericRepository<TEntity> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public virtual async Task<ActionResult<ApiResponse<IReadOnlyList<TResultDto>>>> GetAll()
        {
            try
            {
                var entities = await _repo.ListAllAsync();
                var data = _mapper.Map<IReadOnlyList<TResultDto>>(entities);
                return Ok(new ApiResponse<IReadOnlyList<TResultDto>>(200, data: data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Internal Server Error", ex.Message));
            }
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<ApiResponse<TResultDto>>> GetById(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);
                if (entity == null)
                    return NotFound(new ApiResponse<TResultDto>(404, "Not found"));

                var data = _mapper.Map<TResultDto>(entity);
                return Ok(new ApiResponse<TResultDto>(200, data: data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Internal Server Error", ex.Message));
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult<ApiResponse<TResultDto>>> Create([FromBody] TCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new ApiValidationErrorResponse
                {
                    Errors = errors
                });
            }

            try
            {
                var entity = _mapper.Map<TEntity>(dto);
                _repo.Add(entity);
                await _repo.SaveChangesAsync();

                var created = _mapper.Map<TResultDto>(entity);
                return CreatedAtAction(nameof(GetById), new { id = GetEntityId(entity) },
                    new ApiResponse<TResultDto>(201, data: created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Internal Server Error", ex.Message));
            }
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<ApiResponse<TResultDto>>> Update(Guid id, [FromBody] TUpdateDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return NotFound(new ApiResponse<TResultDto>(404, "Not found"));

            try
            {
                _mapper.Map(dto, entity); // Id sẽ không bị map vì UpdateDto không có Id
                await _repo.SaveChangesAsync();

                var updated = _mapper.Map<TResultDto>(entity);
                return Ok(new ApiResponse<TResultDto>(200, data: updated));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Internal Server Error", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return NotFound(new ApiResponse<string>(404, "Not found"));

            try
            {
                _repo.Remove(entity);
                await _repo.SaveChangesAsync();
                return Ok(new ApiResponse<string>(200, message: "Deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiException(500, "Internal Server Error", ex.Message));
            }
        }

        protected virtual object? GetEntityId(TEntity entity)
        {
            var prop = typeof(TEntity).GetProperty("Id");
            return prop?.GetValue(entity);
        }
    }
}
