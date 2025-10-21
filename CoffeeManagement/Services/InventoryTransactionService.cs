using AutoMapper;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.DTOs.InventoryTransaction;
using CoffeeManagement.Interface;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Services
{

    public class InventoryTransactionService : IInventoryTransactionService
    {
        private readonly IGenericRepository<InventoryTransaction> _transactionRepo;
        private readonly IGenericRepository<Ingredient> _ingredientRepo;
        private readonly IMapper _mapper;

        public InventoryTransactionService(
            IGenericRepository<InventoryTransaction> transactionRepo,
            IGenericRepository<Ingredient> ingredientRepo,
            IMapper mapper)
        {
            _transactionRepo = transactionRepo;
            _ingredientRepo = ingredientRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InventoryTransactionResultDto>> Get()
        {
            var transactions = await _transactionRepo.FindAllAsync(
                predicate: t => true,
                include: query => query.Include(t => t.Ingredient)
                               .OrderByDescending(t => t.CreatedAt)
    );

            return _mapper.Map<IEnumerable<InventoryTransactionResultDto>>(transactions);
        }

        public async Task<InventoryTransactionResultDto> GetById(Guid id)
        {
            var transaction = await _transactionRepo.FindSingleAsync(
                predicate: t => t.Id == id,
                include: query => query.Include(t => t.Ingredient)
            );

            return _mapper.Map<InventoryTransactionResultDto>(transaction);
        }

        public async Task<InventoryTransactionResultDto> Create(CreateInventoryTransactionDto dto)
        {
            var ingredient = await _ingredientRepo.GetByIdAsync(dto.IngredientId);

            // Cập nhật CurrentStock dựa vào TransactionType
            UpdateIngredientStock(ingredient, dto.TransactionType, dto.Quantity);

            var transaction = _mapper.Map<InventoryTransaction>(dto);
            transaction.Id = Guid.NewGuid();
            transaction.CreatedAt = DateTime.UtcNow;

            _transactionRepo.Add(transaction);
            _ingredientRepo.Update(ingredient);

            await _transactionRepo.SaveChangesAsync();

            return await GetById(transaction.Id);
        }

        public async Task<InventoryTransactionResultDto> Update(Guid id, UpdateInventoryTransactionDto dto)
        {
            var existingTransaction = await _transactionRepo.GetByIdAsync(id);
            var ingredient = await _ingredientRepo.GetByIdAsync(existingTransaction.IngredientId);

            RevertIngredientStock(ingredient, existingTransaction.TransactionType, existingTransaction.Quantity);

            UpdateIngredientStock(ingredient, dto.TransactionType, dto.Quantity);
            _mapper.Map(dto, existingTransaction);
            existingTransaction.UpdatedAt = DateTime.UtcNow;

            _transactionRepo.Update(existingTransaction);
            _ingredientRepo.Update(ingredient);

            await _transactionRepo.SaveChangesAsync();

            return await GetById(id);
        }

        public async Task<bool> Delete(Guid id)
        {
            var transaction = await _transactionRepo.GetByIdAsync(id);
            var ingredient = await _ingredientRepo.GetByIdAsync(transaction.IngredientId);

            RevertIngredientStock(ingredient, transaction.TransactionType, transaction.Quantity);

            _transactionRepo.Remove(transaction);
            _ingredientRepo.Update(ingredient);

            await _transactionRepo.SaveChangesAsync();

            return true;
        }

        private void UpdateIngredientStock(Ingredient ingredient, string transactionType, decimal quantity)
        {
            switch (transactionType.ToUpper())
            {
                case "IN": // Nhập kho
                    ingredient.CurrentStock += quantity;
                    break;

                case "OUT": // Xuất kho
                    ingredient.CurrentStock -= quantity;
                    if (ingredient.CurrentStock < 0)
                        throw new InvalidOperationException("Số lượng tồn kho không đủ");
                    break;

                case "ADJUSTMENT": // Điều chỉnh (có thể âm hoặc dương)
                    ingredient.CurrentStock += quantity;
                    if (ingredient.CurrentStock < 0)
                        throw new InvalidOperationException("Số lượng tồn kho không hợp lệ sau điều chỉnh");
                    break;

                case "LOSS": // Hao hụt/Mất mát
                    ingredient.CurrentStock -= quantity;
                    if (ingredient.CurrentStock < 0)
                        throw new InvalidOperationException("Số lượng tồn kho không đủ");
                    break;

                default:
                    throw new InvalidOperationException($"Loại giao dịch không hợp lệ: {transactionType}");
            }
        }

        private void RevertIngredientStock(Ingredient ingredient, string transactionType, decimal quantity)
        {
            switch (transactionType.ToUpper())
            {
                case "IN": // Hoàn tác nhập kho
                    ingredient.CurrentStock -= quantity;
                    break;

                case "OUT": // Hoàn tác xuất kho
                    ingredient.CurrentStock += quantity;
                    break;

                case "ADJUSTMENT": // Hoàn tác điều chỉnh
                    ingredient.CurrentStock -= quantity;
                    break;

                case "LOSS": // Hoàn tác hao hụt
                    ingredient.CurrentStock += quantity;
                    break;
            }
        }
    }
}