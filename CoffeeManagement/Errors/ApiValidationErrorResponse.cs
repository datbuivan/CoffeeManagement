namespace CoffeeManagement.Errors
{
    public class ApiValidationErrorResponse : ApiResponse<Object>
    {
        public ApiValidationErrorResponse() : base(400)
        {
        }

        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }
}
