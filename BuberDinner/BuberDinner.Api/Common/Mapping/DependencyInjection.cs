using System.Reflection;
using Mapster;
using MapsterMapper;

namespace BuberDinner.Api.Common.Mapping;

public static class DependencyInjection
{
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        // first scan assembly, find all IRegister interfaces and register them
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>(); // have to use service mapper
        return services;
    }
}
