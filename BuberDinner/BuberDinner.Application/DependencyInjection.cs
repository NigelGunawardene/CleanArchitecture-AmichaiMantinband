//using BuberDinner.Application.Services.Authentication.Commands;
//using BuberDinner.Application.Services.Authentication.Queries;
using System.Reflection;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Common.Behvaiors;
using BuberDinner.Application.Services.Authentication.Common;
using ErrorOr;
using FluentValidation;
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
        services.AddScoped<IPipelineBehavior<RegisterCommand, ErrorOr<AuthenticationResult>>, ValidateRegisterCommandBehavior>();
        //services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}
