using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// add services to container

builder.Services
    .AddApplicationServices()
    .AddInfrastrutureServices(builder.Configuration)
    .AddApiServices();



var app = builder.Build();

// configure http request pipeline
app.UseApiServices();

app.Run();
