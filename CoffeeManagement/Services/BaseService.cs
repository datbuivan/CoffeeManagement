using CoffeeManagement.Errors;
using CoffeeManagement.Interface;
using CoffeeManagement.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Services
{
    public abstract class BaseService<T> : IBaseService<T>
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly HttpClient Client;
        protected readonly string BaseUrl;
        protected readonly ILogger<BaseService<T>> Log;

        protected BaseService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            // Lấy HttpClient từ IHttpClientFactory (nếu có đăng ký trong DI)
            Client = ServiceProvider.GetService<IHttpClientFactory>()?.CreateClient(GetType().Name) ?? new HttpClient();

            // BaseUrl mặc định lấy từ tên kiểu DTO, anh có thể override trong class con nếu cần
            BaseUrl = typeof(T).Name.Replace("Dto", "", StringComparison.OrdinalIgnoreCase);

            Log = ServiceProvider.GetRequiredService<ILogger<BaseService<T>>>();
        }

        public virtual Task<ReadApiResult<T>> Get() =>
            Get(new QueryString());

        public virtual Task<ReadApiResult<T>> Get(QueryString queryString) =>
            Client.GetContentAsync<ReadApiResult<T>>($"{BaseUrl}{queryString}");

        public virtual Task<T> Get(string id) =>
            Client.GetContentAsync<T>($"{BaseUrl}/{id}");

        public virtual Task<T> Add(T f) =>
            Client.PostContentAsync<T>($"{BaseUrl}", f);

        public virtual Task<int> AddRange(IEnumerable<T> models) =>
            Client.PostContentAsync<int>($"{BaseUrl}/AddRange", models);

        public virtual Task<int> UpdateRange(IEnumerable<T> models) =>
            Client.PostContentAsync<int>($"{BaseUrl}/UpdateRange", models);

        public virtual Task<T> Update(T f) =>
            Client.PutContentAsync<T>($"{BaseUrl}", f);

        public virtual Task Delete(string id) =>
            Client.DeleteContentAsync($"{BaseUrl}/{id}");
    }
}
