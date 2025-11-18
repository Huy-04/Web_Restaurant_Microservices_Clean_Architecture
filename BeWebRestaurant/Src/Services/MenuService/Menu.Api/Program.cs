using Api.Core.Logging;
using Api.Core.Middleware;
using MediatR;
using Menu.Application.Modules.FoodType.Commands.CreateFoodType;
using Menu.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
builder.Host.UseSharedSerilog(builder.Configuration);

// Add controllers
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Menu API",
        Version = "v1",
        Description = "API for managing MenuService",
        Contact = new OpenApiContact
        {
            Name = "Beer",
            Email = "danhuy@gmail.com",
            Url = new Uri("https://example.com")
        }
    });
});

// Add infrastructure (DbContext, repositories, UoW)
builder.Services.AddMenuInfrastructure(builder.Configuration);

// Add mediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateFoodTypeCommand).Assembly);
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