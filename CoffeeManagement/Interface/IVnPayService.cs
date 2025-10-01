using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Models;

namespace CoffeeManagement.Interface
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(OrderDto order, string clientIpAddr, string returnUrl);
        VnPayResponseDto ProcessPaymentResponse(Dictionary<string, string> vnpayData);
    }
}
