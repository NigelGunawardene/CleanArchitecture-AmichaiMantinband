using BuberDinner.Application;
using BuberDinner.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    // these are static methods in other projects that configure the DI container with services that are contained there. This is to avoid dependencies.
    builder.Services.AddApplication().AddInfrastructure();
    //builder.Services.AddApplication();
    //builder.Services.AddInfrastructure();
    builder.Services.AddControllers(); // use for config and dependency injection
}

var app = builder.Build();
{
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

