namespace CoffeeManagement.Models
{
    public class VnPayResponseDto
    {
        public bool IsSuccess { get; set; }
        public bool IsValidSignature { get; set; }
        public Guid OrderId { get; set; }
        public string VnPayTranId { get; set; } = null!;
        public string ResponseCode { get; set; } = null!;
        public string TransactionStatus { get; set; } = null!;
        public string OrderInfo { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime PayDate { get; set; }
    }
}
