using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.DeleteProduct;
using Catalog.API.Products.GetProductByCategory;
using Catalog.API.Products.GetProductById;
using Catalog.API.Products.GetProducts;
using Catalog.API.Products.UpdateProduct;
using FluentValidation;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// add services to container

builder.Services.AddCarter(configurator: config =>
{
    config.WithModule<CreateProductEndpoint>();
    config.WithModule<GetProductsEndpoint>();
    config.WithModule<GetProductByIdEndpoint>();
    config.WithModule<GetProductByCategoryEndpoint>();
    config.WithModule<UpdateProductEndpoint>();
    config.WithModule<DeleteProductEndpoint>();
});
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddMarten(config =>
{
    config.Connection(builder.Configuration.GetConnectionString("Database")!);
    //config.AutoCreateSchemaObjects
}).UseLightweightSessions();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);


var app = builder.Build();


// configure  the HTTP  request pipeline
app.MapCarter();
app.UseExceptionHandler(opt =>
{

});

app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
