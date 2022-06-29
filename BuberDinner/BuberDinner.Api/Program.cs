using BuberDinner.Api.Errors;
using BuberDinner.Api.Filters;
using BuberDinner.Api.Middleware;
using BuberDinner.Application;
using BuberDinner.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    // these are static methods in other projects that configure the DI container with services that are contained there. This is to avoid dependencies.
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
    //builder.Services.AddApplication();
    //builder.Services.AddInfrastructure();

    // disabling this as well (after middleware)
    //builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>()); // use for config and dependency injection

    builder.Services.AddControllers();

    builder.Services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();
}

var app = builder.Build();
{
    // commenting this out in order to not use the middleware approach for now
    //app.UseMiddleware<ErrorHandlingMiddleware>();

    // 3rd approach to error handling - error endpoint
    app.UseExceptionHandler("/error"); // this is used in conjunction with the factory 


    // Instead of doing builder.Services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();, we can also do:
    //app.Map("/error", (HttpContext httpContext) =>
    //{
    //    Exception? exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
    //    return Results.Problem();
    //});

    app.UseHttpsRedirection();
    app.MapControllers();

    app.Run();
}
















// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseAuthorization();

