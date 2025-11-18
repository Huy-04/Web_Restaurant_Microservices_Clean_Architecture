using Api.Core.Logging;
using Api.Core.Middleware;
using Inventory.Application.Modules.Stock.Commands.CreateStock;
using Inventory.Infrastructure;
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

// Add infrastructure (DbContext, repositories, UoW)
builder.Services.AddInventoryInfrastructure(builder.Configuration);

// Add mediaR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateStockCommand).Assembly);
});

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