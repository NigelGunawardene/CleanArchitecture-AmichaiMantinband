using BuberDinner.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

// we use this class to register our authentication services in DI because we do not want to do it in the API project

namespace BuberDinner.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        return services;
    }
}
