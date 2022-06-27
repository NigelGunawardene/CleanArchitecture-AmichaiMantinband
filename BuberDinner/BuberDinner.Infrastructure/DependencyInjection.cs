using Microsoft.Extensions.DependencyInjection;

// we use this class to register our authentication services in DI because we do not want to do it in the API project

namespace BuberDinner.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services;
    }
}
