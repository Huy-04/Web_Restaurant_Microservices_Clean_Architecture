using Api.Core.Logging;
using Api.Core.Middleware;
using Application.Core.Behaviors;
using Inventory.Application.Interfaces;
using Inventory.Application.Modules.Stock.Commands.CreateStock;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
builder.Host.UseSharedSerilog(builder.Configuration);

// Add controllers
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Inventory API",
        Version = "v1",
        Description = "API for managing InventoryService",
        Contact = new OpenApiContact
        {
            Name = "Beer",
            Email = "danhuy@gmail.com",
            Url = new Uri("https://example.com")
        }
    });
});

// Add Dbcontext
builder.Services.AddDbContext<InventoryDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("InventoryDatabase");
    options.UseSqlServer(cs);
});

// Add repository & unitofword
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IStockItemsRepository, StockItemsRepository>();
builder.Services.AddScoped<IIngredientsRepository, IngredientsRepository>();
builder.Services.AddScoped<IFoodRecipesRepository, FoodRecipesRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add mediaR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateStockCommand).Assembly);
});

// Add behaviors
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));

// Build app
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();