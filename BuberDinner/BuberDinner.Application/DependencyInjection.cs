//using BuberDinner.Application.Services.Authentication.Commands;
//using BuberDinner.Application.Services.Authentication.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

// we use this class to register our authentication services in DI because we do not want to do it in the API project

namespace BuberDinner.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //services.AddScoped<IAuthenticationCommandService, AuthenticationCommandService>();
        //services.AddScoped<IAuthenticationQueryService, AuthenticationQueryService>();
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        return services;
    }
}
