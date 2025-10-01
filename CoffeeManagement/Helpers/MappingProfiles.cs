using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.Category;
using CoffeeManagement.Data.Dtos.Product;
using CoffeeManagement.Data.Dtos.ProductSize;
using CoffeeManagement.Data.Dtos.Table;
using CoffeeManagement.Data.Entities;

namespace CoffeeManagement.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Category
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<Category, CategoryResultDto>();

            // Product
            CreateMap<Product, ProductResultDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();

            // Ingredient
            CreateMap<Ingredient, IngredientDto>().ReverseMap();

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

            // Table
            CreateMap<TableCreateDto, Table>();
            CreateMap<TableUpdateDto, Table>();
            CreateMap<Table, TableResultDto>();

            //ProductSize 
            CreateMap<ProductSizeCreateDto, ProductSize>();
            CreateMap<ProductSizeUpdateDto, ProductSize>();
            CreateMap<ProductSize, ProductSizeResultDto>();
        }
    }
}
