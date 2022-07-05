using BuberDinner.Api.Errors;
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

    builder.Services.AddControllers();

    builder.Services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();
}

var app = builder.Build();
{

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

