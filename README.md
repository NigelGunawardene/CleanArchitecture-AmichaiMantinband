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


# Chapters

### Chapter 1

Setup

### Chapter 2

Authentication - generating JWT Tokens

### Chapter 3

Repository pattern

### Chapter 4

Global error handling

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


### Chapter 5
Flow control

Removed filters and middleware error handling as they are not needed. Only using endpoint approach - ErrorController

FOUR appraoches to flow control

1 - via Exceptions - not that good.

2 - used at Microsoft and fairly common - 
Install OneOf package into application project
Create IError interface, extend it in DuplicateEmailEror and use it in the controller. This is nice.

3 - 
Install FluentResults into application project
Similar to OneOf it is a Discriminated Union
Instead of us having to define more and more results, it will always contain either the class that is defined, or a list of errors. You can either return an object that we define because it has an implicit converter from an authenticationresult, or return an error 

4 - Using ErrorOr and domain level exceptions
Install ErrorOr into domain project



# This handles all the secrets automatically overriding them


dotnet user-secrets init --project .\BuberDinner.Api\

dotnet user-secrets set --project .\BuberDinner.Api\ "JwtSettings:Secret"  "super-secret-key-from-user-secrets"

dotnet user-secrets list --project .\BuberDinner.Api\

# To debug in VS Code, go to run and debug and create json thingy. (missing assets popup when you open for the first time). Then click attach and select BuberDinner.Api process
    

### Chapter 6

In chapter 6, first we split the AuthenticationService into Commands and queries - AuthenticationCommandService and AuthenticationQueryService.

However, these are still services. It follows the CQRS pattern(Am I changing anything?)

Chapter 6 - part 2 - we implement Mediatr. We create commands, queries and handlers.
Create RegisterCommandHandler, implement relevant interface, copy logic from register service. Can get rid of the Services folder with Authenticaion

Important to note that at the moment, in the LoginQuery and AuthenticationResult, we return User. User is a domain entity (and potential aggregate) but one of the main motications for CQRS in DDD is to have the response as slim as possible. Therefore we will implement a slim UserDto to use here with only the necessary data


























