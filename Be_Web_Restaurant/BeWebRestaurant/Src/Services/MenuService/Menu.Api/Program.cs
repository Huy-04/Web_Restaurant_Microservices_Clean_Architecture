using Common.Behaviors;
using Common.Logging;
using Common.Middleware;
using MediatR;
using Menu.Application.Interfaces;
using Menu.Application.Modules.FoodTypes.Commands.CreateFoodType;
using Menu.Infrastructure.Persistence;
using Menu.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
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

// Add DbContext

builder.Services.AddDbContext<MenuDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("MenuDatabase");
    options.UseSqlServer(cs);
    options.EnableSensitiveDataLogging(false);
});

// Add repository & unitofword
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IFoodTypeRepository, FoodTypeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add mediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateFoodTypeCommand).Assembly);
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