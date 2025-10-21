
using CoffeeManagement.Data.Entities;
using CoffeeManagement.DTOs.InventoryTransaction;
using CoffeeManagement.Interface;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Interface
{
    public interface IInventoryTransactionService
    {
        Task<IEnumerable<InventoryTransactionResultDto>> Get();
        Task<InventoryTransactionResultDto> GetById(Guid id);
        Task<InventoryTransactionResultDto> Create(CreateInventoryTransactionDto transaction);
        Task<InventoryTransactionResultDto> Update(Guid id, UpdateInventoryTransactionDto transaction);
        Task<bool> Delete(Guid id);
    }
}