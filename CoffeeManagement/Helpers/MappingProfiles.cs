using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Entities;

namespace CoffeeManagement.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Category
            CreateMap<Category, CategoryDto>().ReverseMap();

            // Product
            CreateMap<Product, ProductDto>().ReverseMap();

            // Ingredient
            CreateMap<Ingredient, IngredientDto>().ReverseMap();

            // ProductIngredient
            CreateMap<ProductIngredient, ProductIngredientDto>().ReverseMap();

            // Order
            CreateMap<Order, OrderDto>().ReverseMap();

            // OrderItem
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();

            // InventoryTransaction
            CreateMap<InventoryTransaction, InventoryTransactionDto>().ReverseMap();

            // ReportDailyRevenue
            CreateMap<ReportDailyRevenue, ReportDailyRevenueDto>().ReverseMap();

            // ReportInventorySummary
            CreateMap<ReportInventorySummary, ReportInventorySummaryDto>().ReverseMap();
        }
    }
}
