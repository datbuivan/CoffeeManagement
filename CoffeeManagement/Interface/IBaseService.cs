using CoffeeManagement.Errors;

namespace CoffeeManagement.Interface
{
    public interface IBaseService<T>
    {
        Task<ReadApiResult<T>> Get();
        Task<ReadApiResult<T>> Get(QueryString queryString);
        Task<T> Get(string id);
        Task Delete(string id);
        Task<T> Add(T f);
        Task<T> Update(T f);
        Task<int> AddRange(IEnumerable<T> models);
        Task<int> UpdateRange(IEnumerable<T> models);
    }
}
