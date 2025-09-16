namespace CoffeeManagement.Errors
{
    public class ReadApiResult<T>
    {
        public int TotalCount { get; set; }
        public int Count => Data?.Count() ?? 0;
        public IEnumerable<T> Data { get; set; }
        public object GroupData { get; set; }
    }
}
