using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using CoffeeManagement.Models;

namespace CoffeeManagement.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IProductSizeRepository _productSizeRepo;
        private readonly IGenericRepository<OrderItem> _orderItemRepo;
        private readonly IUnitOfWork _unit;
        private readonly IVnPayService _vnPayService;
        public OrderService(IGenericRepository<Order> orderRepo, IGenericRepository<Product> productRepo, IProductSizeRepository productSizeRepo, IGenericRepository<OrderItem> orderItemRepo, IUnitOfWork unit, IVnPayService vnPayService)
        {
            _orderItemRepo = orderItemRepo;
            _productRepo = productRepo;
            _productSizeRepo = productSizeRepo;
            _orderRepo = orderRepo;
            _unit = unit;
            _vnPayService = vnPayService;
        }
        public async Task<OrderDto> Create(string userId, int? tableNumber, List<(Guid productId, Guid ProductSizeId, int quantity)> items)
        {
            using (var transaction = await _unit.BeginTransactionAsync())
            {
                try
                {
                    var order = new Order
                    {
                        UserId = userId,
                        //TableNumber = tableNumber,
                        Status = "Pending",
                        TotalAmount = 0
                    };

                    _orderRepo.Add(order);
                    foreach (var item in items)
                    {
                        var product = await _productRepo.GetByIdAsync(item.productId);
                        if (product == null)
                            throw new Exception($"Sản phẩm {item.productId} không tồn tại.");
                        var productSize = await _productSizeRepo.GetByIdAndProductId(item.ProductSizeId, item.productId);
                        var orderItem = new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = product.Id,
                            Quantity = item.quantity,
                            UnitPrice = productSize.Price,
                            SubTotal = productSize.Price * item.quantity
                        };
                        order.TotalAmount += orderItem.SubTotal;
                        _orderItemRepo.Add(orderItem);
                    }
                    await _unit.Complete();
                    await transaction.CommitAsync();

                    return MapToDto(order);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<IEnumerable<OrderDto>> Get()
        {
            var orders = await _orderRepo.ListAllAsync();
            var result = new List<OrderDto>();

            foreach (var order in orders)
            {
                ;
                result.Add(MapToDto(order));
            }

            return result;
        }

        public async Task<OrderDto?> GetById(Guid orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null) return null;
            return MapToDto(order);
        }

        public async Task<string> CreatePaymentUrl(Guid orderId, string clientIpAddr, string returnUrl)
        {
            var order = await GetById(orderId);
            if (order == null)
                throw new Exception($"Đơn hàng {orderId} không tồn tại.");

            if (order.Status != "Pending")
                throw new Exception("Chỉ có thể thanh toán đơn hàng đang chờ xử lý.");

            return _vnPayService.CreatePaymentUrl(order, clientIpAddr, returnUrl);
        }

        public async Task<bool> ProcessPaymentResponse(VnPayResponseDto response)
        {
            if (!response.IsSuccess)
                return false;

            using var transaction = await _unit.BeginTransactionAsync();
            try
            {
                var order = await _orderRepo.GetByIdAsync(response.OrderId);
                if (order == null)
                    return false;

                // Cập nhật trạng thái đơn hàng thành "Paid"
                order.Status = "Paid";
                order.UpdatedAt = DateTime.Now;

                _orderRepo.Update(order);
                await _unit.Complete();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<OrderDto?> UpdateOrderStatus(Guid orderId, string status)
        {
            using var transaction = await _unit.BeginTransactionAsync();
            try
            {
                var order = await _orderRepo.GetByIdAsync(orderId);
                if (order == null) return null;

                order.Status = status;

                _orderRepo.Update(order);
                await _unit.Complete();
                await transaction.CommitAsync();

                return MapToDto(order);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                //TableNumber = order.TableNumber,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
            };
        }

    }
}
