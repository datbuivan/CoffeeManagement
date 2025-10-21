using CoffeeManagement.Data;
using CoffeeManagement.Data.Entities.Custom;
using CoffeeManagement.Errors;
using CoffeeManagement.Interface;
using CoffeeManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Services
{
    public abstract class BaseService<TDto, TEntity> : IBaseService<TDto>
        where TEntity : BaseEntities
    {
        //protected readonly IGenericRepository<TEntity> _repository;
        //protected readonly ILogger<BaseService<TDto, TEntity>> _log;

        //protected BaseService(IGenericRepository<TEntity> repository, ILogger<BaseService<TDto, TEntity>> log)
        //{
        //    _repository = repository;
        //    _log = log;
        //}

        //public virtual async Task<ReadApiResult<TDto>> Get()
        //{
        //    var entities = await _repository.ListAllAsync();
        //    var dtos = entities.Select(MapToDto);
        //    return new ReadApiResult<TDto>
        //    {
        //        Data = dtos,
        //        TotalCount = dtos.Count()
        //    };
        //}

        //public virtual async Task<ReadApiResult<TDto>> Get(QueryString queryString)
        //{
        //    // TODO: parse queryString để filter, sort, paging nếu muốn
        //    var entities = await _repository.ListAllAsync();
        //    var dtos = entities.Select(MapToDto);
        //    return new ReadApiResult<TDto>
        //    {
        //        Data = dtos,
        //        TotalCount = dtos.Count()
        //    };
        //}

        //public virtual async Task<TDto> Get(string id)
        //{
        //    if (!int.TryParse(id, out var intId))
        //        return default;

        //    var entity = await _repository.GetByIdAsync(intId);
        //    return entity == null ? default : MapToDto(entity);
        //}

        //public virtual async Task<TDto> Add(TDto dto)
        //{
        //    var entity = MapToEntity(dto);
        //    _repository.Add(entity);
        //    // giả sử repository thao tác UnitOfWork hoặc DbContext bên ngoài
        //    return MapToDto(entity);
        //}

        //public virtual async Task<int> AddRange(IEnumerable<TDto> dtos)
        //{
        //    var entities = dtos.Select(MapToEntity);
        //    _repository.AddRange(entities);
        //    return entities.Count();
        //}

        //public virtual async Task<TDto> Update(TDto dto)
        //{
        //    var entity = await _repository.GetByIdAsync(GetId(dto));
        //    if (entity == null) return default;

        //    UpdateEntity(entity, dto);
        //    _repository.Update(entity);
        //    return MapToDto(entity);
        //}

        //public virtual async Task<int> UpdateRange(IEnumerable<TDto> dtos)
        //{
        //    int count = 0;
        //    foreach (var dto in dtos)
        //    {
        //        var entity = await _repository.GetByIdAsync(GetId(dto));
        //        if (entity != null)
        //        {
        //            UpdateEntity(entity, dto);
        //            _repository.Update(entity);
        //            count++;
        //        }
        //    }
        //    return count;
        //}

        //public virtual async Task Delete(string id)
        //{
        //    if (!int.TryParse(id, out var intId))
        //        return;

        //    var entity = await _repository.GetByIdAsync(intId);
        //    if (entity != null)
        //    {
        //        _repository.Remove(entity);
        //    }
        //}

        //// ---------------- Abstract methods ----------------
        //protected abstract TDto MapToDto(TEntity entity);
        //protected abstract TEntity MapToEntity(TDto dto);
        //protected abstract int GetId(TDto dto);
        //protected abstract void UpdateEntity(TEntity entity, TDto dto);
    }
}
