using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// add services to container

builder.Services
    .AddApplicationServices()
    .AddInfrastrutureServices(builder.Configuration)
    .AddApiServices();



var app = builder.Build();

// configure http request pipeline
app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.Run();
