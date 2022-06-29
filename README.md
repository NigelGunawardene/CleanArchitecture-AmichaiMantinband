# CleanArchitecture-AmichaiMantinband

Clean architecture following this course - https://www.youtube.com/channel/UClz49zOCnzsclUJY-t62lIw/videos

---

Presentation -> Application -> Domain (Infrastructure connects to Application Layer) - Jason Taylors

---

Presentation <-> Infrastructure -> DB
      |                |
        Application
            |
        Domain

---

Presentation layer - Contracts and API (only webapi)

Infrastructure layer - Infrastructure

Application layer - Authentication

Domain

---

Commands to create -

dotnet new sln -o BuberDinner

cd .\BuberDinner\

dotnet new webapi -o BuberDinner.Api

dotnet new classlib -o BuberDinner.Contracts

dotnet new classlib -o BuberDinner.Infrastructure

dotnet new classlib -o BuberDinner.Application

dotnet new classlib -o BuberDinner.Domain

dotnet build - fails here

more .\BuberDinner.sln - more details

dotnet sln add (ls -r **\*.csproj) - recursively adds all projects in directory to sln

dotnet build

// create dependencies now
// API needs to know about contracts and application
dotnet add .\BuberDinner.Api\ reference .\BuberDinner.Contracts\ .\BuberDinner.Application\ 

// Infrastructure needs to know about application
dotnet add .\BuberDinner.Infrastructure\ reference .\BuberDinner.Application\ 

// Application needs to know about domain
dotnet add .\BuberDinner.Application\ reference .\BuberDinner.Domain\ 

// We also need to API to know about Infrastructure layer
dotnet add .\BuberDinner.Api\ reference .\BuberDinner.Infrastructure\ 

code .
dotnet build

dotnet run --project .\BuberDinner.Api\

OR

dotnet watch run --project .\BuberDinner.Api\

---

Install Rest Client and create folders Requests/something.http

dotnet add .\BuberDinner.Application\ package Microsoft.Extensions.DependencyInjection.Abstractions

# This handles all the secrets automatically overriding them


dotnet user-secrets init --project .\BuberDinner.Api\

dotnet user-secrets set --project .\BuberDinner.Api\ "JwtSettings:Secret"  "super-secret-key-from-user-secrets"

dotnet user-secrets list --project .\BuberDinner.Api\

# To debug in VS Code, go to run and debug and create json thingy. (missing assets popup when you open for the first time). Then click attach and select BuberDinner.Api process
    


# Error handling

1. Middleware
Set up middleware but disabled in program.cs. add to program.cs to use
Code is in Middleware/ErrorHandlingMiddlware
app.UseMiddleware<ErrorHandlingMiddleware>();

2. Filters
Set up filter and add attribute to controller to use - [ErrorHandlingFilter] OR in program.cs
Code is in Filters/ErrorHandlingFilterAttribute
builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());

3. endpoint approach. added to program.cs
Code is in Controller/ErrorsController
app.UseExceptionHandler("/error");

4. Error Factory
This is used in conjunction with the endpoint approach. replaced the default factory

Code is in Errors/BuberDinnerProblemDetailsFactory
builder.Services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();


























