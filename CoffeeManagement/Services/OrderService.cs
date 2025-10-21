using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.Order;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using CoffeeManagement.Models;

namespace CoffeeManagement.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IVnPayService _vnPayService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderService(IGenericRepository<Order> orderRepository, IUnitOfWork uow, IMapper mapper, IVnPayService vnPayService, IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _uow = uow;
            _mapper = mapper;
            _vnPayService = vnPayService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OrderResultDto> GetById(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderResultDto>(order);
        }

        public async Task<object> CreateAndPayOrderAsync(OrderAndPayDto dto)
        {
            await using var transaction = await _uow.BeginTransactionAsync();
            try
            {
                var orderItems = new List<OrderItem>();
                decimal totalAmount = 0;

                foreach (var itemDto in dto.Items)
                {
                    var productSize = await _uow.GenericRepository<ProductSize>().GetByIdAsync(itemDto.ProductSizeId);
                    if (productSize == null) throw new Exception($"ProductSize Id {itemDto.ProductSizeId} not found.");

                    var recipes = await _uow.GenericRepository<Recipe>().FindAllAsync(r => r.ProductSizeId == itemDto.ProductSizeId);
                    foreach (var recipeItem in recipes)
                    {
                        var ingredient = await _uow.GenericRepository<Ingredient>().GetByIdAsync(recipeItem.IngredientId);
                        var requiredQuantity = recipeItem.QuantityUsed * itemDto.Quantity;

                        if (ingredient.CurrentStock < requiredQuantity)
                            throw new Exception($"Not enough stock for ingredient: {ingredient.Name}.");

                        ingredient.CurrentStock -= requiredQuantity;
                        _uow.GenericRepository<Ingredient>().Update(ingredient);
                    }

                    var orderItem = new OrderItem
                    {
                        ProductId = productSize.ProductId,
                        ProductSizeId = productSize.Id,
                        Quantity = itemDto.Quantity,
                        UnitPrice = productSize.Price,
                        SubTotal = productSize.Price * itemDto.Quantity,
                    };

                    orderItems.Add(orderItem);
                    totalAmount += orderItem.SubTotal;
                }

                // Bước 2: Tạo đối tượng Order và xử lý theo phương thức thanh toán
                var order = new Order
                {
                    UserId = dto.UserId,
                    TableId = dto.TableId,
                    OrderItems = orderItems,
                    TotalAmount = totalAmount,
                    DiscountAmount = dto.DiscountAmount,
                    FinalAmount = totalAmount - dto.DiscountAmount,
                    CreatedAt = DateTime.UtcNow
                };

                // Xử lý bàn
                if (dto.TableId.HasValue)
                {
                    var table = await _uow.GenericRepository<Table>().GetByIdAsync(dto.TableId.Value);
                    if (table == null) throw new Exception($"Table Id {dto.TableId.Value} not found.");
                    table.Status = "Occupied";
                    _uow.GenericRepository<Table>().Update(table);
                }

                // Xử lý thanh toán
                if (dto.PaymentMethod.Equals("Cash", StringComparison.OrdinalIgnoreCase))
                {
                    // Thanh toán tiền mặt: Chuyển trạng thái thành "Paid" ngay lập tức
                    order.Status = "Paid";
                    _uow.GenericRepository<Order>().Add(order);
                    await _uow.Complete();
                    await transaction.CommitAsync();

                    // Trả về thông tin đơn hàng đã hoàn tất
                    var orderResult = _mapper.Map<OrderResultDto>(order);
                    return orderResult;
                }
                else if (dto.PaymentMethod.Equals("VnPay", StringComparison.OrdinalIgnoreCase))
                {
                    // Thanh toán VNPAY: Chuyển trạng thái thành "Chờ thanh toán"
                    // Điều này quan trọng để bếp không pha chế món chưa được trả tiền
                    order.Status = "WaitingForPayment";
                    _uow.GenericRepository<Order>().Add(order);
                    await _uow.Complete();
                    await transaction.CommitAsync();

                    // var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
                    var ipAddress = "127.0.0.1";
                    var paymentUrl = _vnPayService.CreatePaymentUrl(order, ipAddress);

                    // Trả về URL để client chuyển hướng
                    return new { PaymentUrl = paymentUrl };
                }
                else
                {
                    throw new Exception("Invalid payment method.");
                }
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    try
                    {
                        await _uow.RollbackAsync();
                    }
                    catch
                    {
                        // Transaction đã commit hoặc disposed, bỏ qua lỗi rollback
                    }
                }
                throw;
            }
        }

        public async Task HandleVnPayCallbackAsync(IQueryCollection responseData)
        {
            // Bước 1: Giao toàn bộ việc xử lý và xác thực cho VnPayService
            VnPayResponseDto vnPayResponse = _vnPayService.ProcessPaymentResponse(responseData);

            if (!vnPayResponse.IsValidSignature)
            {
                throw new Exception("VNPAY Callback: Invalid signature.");
            }

            var order = await _uow.GenericRepository<Order>().GetByIdAsync(vnPayResponse.OrderId);
            if (order == null) throw new Exception("Order not found.");
            if (order.Status == "Paid") return; // Giao dịch đã được xử lý, bỏ qua
            if (order.FinalAmount != vnPayResponse.Amount) throw new Exception("Invalid amount.");

            await using var transaction = await _uow.BeginTransactionAsync();
            try
            {
                if (vnPayResponse.IsSuccess)
                {
                    order.Status = "Paid";
                }
                else
                {
                    order.Status = "PaymentFailed";
                    // Hoàn trả lại số lượng nguyên liệu đã trừ
                    var recipes = await _uow.GenericRepository<Recipe>().FindAllAsync(
                        r => order.OrderItems.Select(oi => oi.ProductSizeId).Contains(r.ProductSizeId)
                    );
                    foreach (var item in order.OrderItems)
                    {
                        var itemRecipes = recipes.Where(r => r.ProductSizeId == item.ProductSizeId);
                        foreach (var recipeItem in itemRecipes)
                        {
                            var ingredient = await _uow.GenericRepository<Ingredient>().GetByIdAsync(recipeItem.IngredientId);
                            ingredient.CurrentStock += recipeItem.QuantityUsed * item.Quantity; // Cộng trả lại
                            _uow.GenericRepository<Ingredient>().Update(ingredient);
                        }
                    }
                }

                _uow.GenericRepository<Order>().Update(order);
                await _uow.Complete();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}
