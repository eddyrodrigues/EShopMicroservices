using Basket.API.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
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
    .AddNpgSql(connectionString);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();


var app = builder.Build();


app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}); 
app.UseExceptionHandler(opt =>{});

app.MapCarter();
app.MapGet("/", () => "Hello World!");

app.Run();
