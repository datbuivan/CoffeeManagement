namespace CoffeeManagement.Utilities
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetContentAsync<T>(this HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>()
                   ?? throw new InvalidOperationException($"Không parse được JSON từ {url}");
        }

        public static async Task<T> PostContentAsync<T>(this HttpClient client, string url, object data)
        {
            var response = await client.PostAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>()
                   ?? throw new InvalidOperationException($"Không parse được JSON từ {url}");
        }

        public static async Task<T> PutContentAsync<T>(this HttpClient client, string url, object data)
        {
            var response = await client.PutAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>()
                   ?? throw new InvalidOperationException($"Không parse được JSON từ {url}");
        }

        public static async Task DeleteContentAsync(this HttpClient client, string url)
        {
            var response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}
