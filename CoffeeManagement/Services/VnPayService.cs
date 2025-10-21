using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Helpers;
using CoffeeManagement.Interface;
using CoffeeManagement.Models;
using System.Globalization;

namespace CoffeeManagement.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;

        public VnPayService(IConfiguration config)
        {
            _config = config;
        }

        public string CreatePaymentUrl(Order order, string clientIpAddr)
        {
            var vnpay = new VnPay();

            var vnp_Url = _config["VnPay:BaseUrl"];
            var vnp_TmnCode = _config["VnPay:TmnCode"];
            var vnp_HashSecret = _config["VnPay:HashSecret"];
            var vnp_ReturnUrl = _config["VnPay:ReturnUrl"];

            if (string.IsNullOrEmpty(vnp_Url) || string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret) || string.IsNullOrEmpty(vnp_ReturnUrl))
            {
                throw new Exception("VnPay configuration is missing from appsettings.json");
            }

            vnpay.AddRequestData("vnp_Version", VnPay.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", ((long)(order.TotalAmount * 100)).ToString()); // VnPay tính theo VND * 100
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_TxnRef", order.Id.ToString());
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {order.Id}");
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
            vnpay.AddRequestData("vnp_IpAddr", clientIpAddr);
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

            var paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return paymentUrl;
        }

        public VnPayResponseDto ProcessPaymentResponse(IQueryCollection vnpayData)
        {
            var vnpay = new VnPay();
            var vnp_HashSecret = _config["VnPay:HashSecret"];
            if (string.IsNullOrEmpty(vnp_HashSecret))
            {
                throw new Exception("VnPay HashSecret configuration is missing.");
            }

            foreach (var (key, value) in vnpayData)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_SecureHash = vnpay.GetResponseData("vnp_SecureHash");
            bool isValidSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            if (!isValidSignature)
            {
                return new VnPayResponseDto { IsValidSignature = false, IsSuccess = false };
            }

            try
            {
                var responseCode = vnpay.GetResponseData("vnp_ResponseCode");
                var transactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");

                return new VnPayResponseDto
                {
                    OrderId = Guid.Parse(vnpay.GetResponseData("vnp_TxnRef")),
                    VnPayTranId = vnpay.GetResponseData("vnp_TransactionNo"),
                    Amount = Convert.ToDecimal(vnpay.GetResponseData("vnp_Amount")) / 100,
                    ResponseCode = responseCode,
                    TransactionStatus = transactionStatus,
                    OrderInfo = vnpay.GetResponseData("vnp_OrderInfo"),
                    PayDate = DateTime.ParseExact(vnpay.GetResponseData("vnp_PayDate"), "yyyyMMddHHmmss", CultureInfo.InvariantCulture),
                    IsValidSignature = true,
                    IsSuccess = responseCode == "00" && transactionStatus == "00"
                };
            }
            catch (Exception ex)
            {
                // Lỗi xảy ra khi parse dữ liệu từ VNPAY trả về
                return new VnPayResponseDto
                {
                    IsValidSignature = true, // Chữ ký vẫn hợp lệ
                    IsSuccess = false,
                    ResponseCode = "99", // Mã lỗi tự định nghĩa
                    OrderInfo = $"Error parsing response: {ex.Message}"
                };
            }
        }

    }
}
