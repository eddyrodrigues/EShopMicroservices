﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastrutureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("Database");

        return services;
    }
}
