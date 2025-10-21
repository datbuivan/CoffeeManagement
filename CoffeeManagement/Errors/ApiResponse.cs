namespace CoffeeManagement.Errors
{
    public class ApiResponse<T>
    {
        public ApiResponse(int statusCode, string? message = null, T? data = default)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            Data = data;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "OK",
                201 => "Created",
                204 => "No Content",
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate.  Hate leads to career change.",
                _ => "Unknown status code!"
            };
        }
    }
}
