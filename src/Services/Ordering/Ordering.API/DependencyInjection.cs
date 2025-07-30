namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {

        //services.AddCarter();

        var assembly = typeof(Program).Assembly;
        //builder.Services.AddCarter();
        //builder.Services.AddCarter(new DependencyContextAssemblyCatalog([assembly]));

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        });

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication webApplication)
    {

        //webApplication.MapCarter();




        return webApplication;
    }

}