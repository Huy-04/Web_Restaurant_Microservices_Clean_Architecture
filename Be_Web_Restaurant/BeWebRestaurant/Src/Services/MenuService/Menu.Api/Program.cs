using Common.Behaviors;
using Common.Middleware;
using MediatR;
using Menu.Application.Interfaces;
using Menu.Application.Modules.FoodTypes.Commands.CreateFoodType;
using Menu.Infrastructure.Persistence;
using Menu.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .MinimumLevel.Debug()

    // Microsoft logs
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore.Server.Kestrel", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)

    // Hide license & middleware noise
    .MinimumLevel.Override("MediatR", LogEventLevel.Error)
    .MinimumLevel.Override("LuckyPennySoftware.MediatR.License", LogEventLevel.Error)
    .MinimumLevel.Override("Common.Middleware.ExceptionMiddleware", LogEventLevel.Error)
    .MinimumLevel.Override("Common.Behaviors.ValidationBehaviors", LogEventLevel.Information)

    .Enrich.With(new ShortSourceContextEnricher())

    // TraceId
    .Enrich.WithActivityId()

    .WriteTo.Console(outputTemplate:
        "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{TraceId}] [{SourceContextShort}] [{Message:lj}]{NewLine}{Exception}")
    .WriteTo.File(
            path: "logs/be/menulog-be-.txt",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30,
            buffered: false,
            flushToDiskInterval: TimeSpan.FromSeconds(1),
            shared: true,
            outputTemplate:
                "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{TraceId}] [{SourceContextShort}] [{Message:lj}]{NewLine}{Exception}"
    )

    // JSON for ui
    .WriteTo.File(
        formatter: new CompactJsonFormatter(),
        path: "logs/fe/menulog-fe-.clef",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        buffered: false,
        flushToDiskInterval: TimeSpan.FromSeconds(1),
        shared: true
    )

);

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