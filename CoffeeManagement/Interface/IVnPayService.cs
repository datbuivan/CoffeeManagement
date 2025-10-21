using CoffeeManagement.Data.Entities;
using CoffeeManagement.Models;

namespace CoffeeManagement.Interface
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(Order order, string clientIpAddr);
        VnPayResponseDto ProcessPaymentResponse(IQueryCollection vnpayData);
    }
}
