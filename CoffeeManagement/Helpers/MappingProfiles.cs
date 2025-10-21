using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.Category;
using CoffeeManagement.Data.Dtos.Order;
using CoffeeManagement.Data.Dtos.Product;
using CoffeeManagement.Data.Dtos.ProductSize;
using CoffeeManagement.Data.Dtos.Recipe;
using CoffeeManagement.Data.Dtos.Report;
using CoffeeManagement.Data.Dtos.Shift;
using CoffeeManagement.Data.Dtos.StaffSheet;
using CoffeeManagement.Data.Dtos.Table;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.DTOs.InventoryTransaction;
using CoffeeManagement.Models.Ingredient;
using CoffeeManagement.Models.User;

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
            CreateMap<Ingredient, IngredientResultDto>();
            CreateMap<CreateIngredientRequest, Ingredient>();
            CreateMap<UpdateIngredientRequest, Ingredient>();

            // OrderItem
            CreateMap<OrderItemCreateDto, OrderItem>();
            CreateMap<OrderItem, OrderItemResultDto>();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();

            // Order
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, OrderResultDto>();


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

            CreateMap<CreateInventoryTransactionDto, InventoryTransaction>();
            CreateMap<UpdateInventoryTransactionDto, InventoryTransaction>();
            CreateMap<InventoryTransaction, InventoryTransactionResultDto>();

            // Recipe 
            CreateMap<RecipeCreateDto, Recipe>();
            CreateMap<RecipeUpdateDto, Recipe>();
            CreateMap<Recipe, RecipeResultDto>();

            //Shift
            CreateMap<Shift, ShiftResultDto>();
            CreateMap<ShiftCreateUpdateDto, Shift>();

            //StaffShift

            CreateMap<StaffShiftAssignDto, StaffShift>();
            CreateMap<StaffShift, StaffShiftResultDto>()
            .ForMember(dest => dest.Shift, opt => opt.MapFrom(src => src.Shift))
            .ForMember(dest => dest.Staff, opt => opt.MapFrom(src => src.Staff));

            CreateMap<ApplicationUser, StaffInfo>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ReverseMap();


            CreateMap<ApplicationUser, UserResultDto>();
            CreateMap<CreateUserRequest, ApplicationUser>()
                .ForMember(dest => dest.AvatarUrl, opt => opt.Ignore()) // xử lý upload riêng
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<UpdateUserRequest, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
