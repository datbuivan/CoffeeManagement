using AutoMapper;
using CoffeeManagement.Data.Dtos.Order;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using CoffeeManagement.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IGenericRepository<OrderItem> _repository;
        private readonly IMapper _mapper;

        public OrderItemService(IGenericRepository<OrderItem> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<OrderItemResultDto>> GetPendingOrderItemsAsync()
        {
            var entities = await _repository.FindAllAsync(oi => !oi.IsDeliverOrder);

            var dtos = _mapper.Map<IReadOnlyList<OrderItemResultDto>>(entities);
            return dtos;
        }

        public async Task<bool> UpdateIsDeliverOrderAsync(Guid orderItemId, bool isDeliver)
        {
            var entity = await _repository.GetByIdAsync(orderItemId);
            if (entity == null) return false;

            entity.IsDeliverOrder = isDeliver;
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateIsDeliverOrdersAsync(IEnumerable<Guid> orderItemIds, bool isDeliver)
        {
            var entities = await _repository.FindAllAsync(oi => orderItemIds.Contains(oi.Id));

            if (!entities.Any()) return false;

            foreach (var entity in entities)
            {
                entity.IsDeliverOrder = isDeliver;
                _repository.Update(entity);
            }

            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
