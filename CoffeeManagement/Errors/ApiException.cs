namespace CoffeeManagement.Errors
{
    public class ApiException : Exception
    {
        public ApiException(int statusCode, string? message = null, string? details = null)
            : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }

        public int StatusCode { get; set; }
        public string? Details { get; set; }
    }
}
