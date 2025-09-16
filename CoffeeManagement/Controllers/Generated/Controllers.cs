using System;
using Microsoft.AspNetCore.Mvc;
using CoffeeManagement.Data.Dto;
// ReSharper disable once CheckNamespace
namespace CoffeeManagement.Controllers;
[Route("api/[controller]")]
public partial class CategoryController(IServiceProvider serviceProvider) 
    : BaseController<CategoryDto>(serviceProvider);

[Route("api/[controller]")]
public partial class ProductController(IServiceProvider serviceProvider) 
    : BaseController<ProductDto>(serviceProvider);

[Route("api/[controller]")]
public partial class IngredientController(IServiceProvider serviceProvider) 
    : BaseController<IngredientDto>(serviceProvider);

[Route("api/[controller]")]
public partial class ProductIngredientController(IServiceProvider serviceProvider) 
    : BaseController<ProductIngredientDto>(serviceProvider);

[Route("api/[controller]")]
public partial class OrderController(IServiceProvider serviceProvider) 
    : BaseController<OrderDto>(serviceProvider);

[Route("api/[controller]")]
public partial class OrderItemController(IServiceProvider serviceProvider) 
    : BaseController<OrderItemDto>(serviceProvider);

[Route("api/[controller]")]
public partial class InventoryTransactionController(IServiceProvider serviceProvider) 
    : BaseController<InventoryTransactionDto>(serviceProvider);

[Route("api/[controller]")]
public partial class ReportDailyRevenueController(IServiceProvider serviceProvider) 
    : BaseController<ReportDailyRevenueDto>(serviceProvider);

[Route("api/[controller]")]
public partial class ReportInventorySummaryController(IServiceProvider serviceProvider) 
    : BaseController<ReportInventorySummaryDto>(serviceProvider);

[Route("api/[controller]")]
public partial class ReportTopProductController(IServiceProvider serviceProvider) 
    : BaseController<ReportTopProductDto>(serviceProvider);

