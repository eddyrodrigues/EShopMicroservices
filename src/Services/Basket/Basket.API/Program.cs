using Basket.API.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
var assembly = typeof(Program).Assembly;
//builder.Services.AddCarter();
builder.Services.AddCarter(new DependencyContextAssemblyCatalog([assembly]));

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

var connectionString = builder.Configuration.GetConnectionString("Database")!;

builder.Services.AddMarten(c =>
{
    c.Connection(connectionString);
}).UseLightweightSessions();


builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!, name: "Redis", tags: new[] { "redis" });

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis")!;
    //options.InstanceName = "Basket";
});

var app = builder.Build();


app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}); 
app.UseExceptionHandler(opt =>{});

app.MapCarter();
app.MapGet("/", () => "Hello World!");

app.Run();
