using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Helpers;
using CoffeeManagement.Interface;
using CoffeeManagement.Models;

namespace CoffeeManagement.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;

        public VnPayService(IConfiguration config)
        {
            _config = config;
        }

        public string CreatePaymentUrl(OrderDto order, string clientIpAddr, string returnUrl)
        {
            var vnpay = new VnPay();

            // Lấy thông tin cấu hình VnPay từ appsettings.json
            var vnp_Url = _config["VnPay:BaseUrl"];
            var vnp_TmnCode = _config["VnPay:TmnCode"];
            var vnp_HashSecret = _config["VnPay:HashSecret"];

            // Thêm các tham số bắt buộc
            vnpay.AddRequestData("vnp_Version", VnPay.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode!);
            vnpay.AddRequestData("vnp_Amount", ((long)(order.TotalAmount * 100)).ToString()); // VnPay tính theo VND * 100
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_TxnRef", order.Id.ToString()); // Mã giao dịch (OrderId)
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {order.Id}");
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);
            vnpay.AddRequestData("vnp_IpAddr", clientIpAddr);
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

            return vnpay.CreateRequestUrl(vnp_Url!, vnp_HashSecret!);
        }

        public VnPayResponseDto ProcessPaymentResponse(Dictionary<string, string> vnpayData)
        {
            var vnpay = new VnPay();
            var vnp_HashSecret = _config["VnPay:HashSecret"];

            // Thêm tất cả dữ liệu response vào VnPay object
            foreach (var item in vnpayData)
            {
                vnpay.AddResponseData(item.Key, item.Value);
            }

            // Lấy các thông tin cần thiết
            var orderId = Guid.Parse(vnpay.GetResponseData("vnp_TxnRef"));
            var vnpayTranId = vnpay.GetResponseData("vnp_TransactionNo");
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            var vnp_SecureHash = vnpayData["vnp_SecureHash"];

            // Xác thực chữ ký
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret!);

            return new VnPayResponseDto
            {
                OrderId = orderId,
                VnPayTranId = vnpayTranId,
                Amount = Convert.ToDecimal(vnpay.GetResponseData("vnp_Amount")) / 100,
                ResponseCode = vnp_ResponseCode,
                TransactionStatus = vnp_TransactionStatus,
                OrderInfo = vnpay.GetResponseData("vnp_OrderInfo"),
                PayDate = DateTime.ParseExact(vnpay.GetResponseData("vnp_PayDate"), "yyyyMMddHHmmss", null),
                IsValidSignature = checkSignature,
                IsSuccess = checkSignature && vnp_ResponseCode == "00" && vnp_TransactionStatus == "00"
            };
        }

    }
}
