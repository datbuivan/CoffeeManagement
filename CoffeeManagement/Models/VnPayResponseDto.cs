namespace CoffeeManagement.Models
{
    public class VnPayResponseDto
    {
        public Guid OrderId { get; set; }
        public string VnPayTranId { get; set; } = null!;
        public decimal Amount { get; set; }
        public string ResponseCode { get; set; } = null!;
        public string TransactionStatus { get; set; } = null!;
        public string OrderInfo { get; set; } = null!;
        public DateTime PayDate { get; set; }
        public bool IsValidSignature { get; set; }
        public bool IsSuccess { get; set; }
    }
}
