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

BuberDinnerProblemDetailsFactory was created by using the Microsoft aspnetcore github. VS extensions were installed for this. Control shift P and type aspnetcore, can open the source code in VS. Can also clone and run in docker using github containers


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

# To debug in VS Code, go to run and debug and create json. (missing assets popup when you open for the first time). Then click attach and select BuberDinner.Api process
    

### Chapter 6

In chapter 6, first we split the AuthenticationService into Commands and queries - AuthenticationCommandService and AuthenticationQueryService.

However, these are still services. It follows the CQRS pattern(Am I changing anything?)

Chapter 6 - part 2 - we implement Mediatr. We create commands, queries and handlers.
Create RegisterCommandHandler, implement relevant interface, copy logic from register service. Can get rid of the Services folder with Authenticaion

Important to note that at the moment, in the LoginQuery and AuthenticationResult, we return User. User is a domain entity (and potential aggregate) but one of the main motications for CQRS in DDD is to have the response as slim as possible. Therefore we will implement a slim UserDto to use here with only the necessary data

### Chapter 7

Adding Mapster

Mapster is a mapping library. Lets say we have

```json
public record User(
  int Id,
  string FirstName,
  string LastName);

```

AND

```json
public record UserResponse(
  int Id,
  string FirstName,
  string LastName);

```

Then we can just do - 
```csharp
var userResponse = user.Adapt<UserResponse>();
```

We can also set rules and pass a config (or use global config) - 
```csharp
var config = new TypeAdapterConfig();
config.NewConfig<User, UserResponse>().Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}")
var userResponse = user.Adapt<UserResponse>(config);
```


There is also a global config that is public and static. To use - 
```csharp
var config = TypeAdapterConfig.GlobalSettings; OR TypeAdapterConfig<User, UserResponse>.NewConfig().Map......
config.NewConfig<User, UserResponse>().Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}")
```

if you want multiple rules for the same conversion, like user to userresponse, we can use config.ForType

we can ignore non mapped fields by using .IgnoreNonMapped

we can also map conditionally with a 3rd argument, like - 
```csharp
config.NewConfig<User, UserResponse>().Map(
  dest => dest.FullName, 
  src => $"{src.FirstName} {src.LastName}",
  src => src.FirstName.StartsWith("a", StringComparison.OrdinalIgnoreCase))
```


We can also combine objects when mapping like using a Tuple - 
```csharp
TypeAdapterConfig<(User User, Guid TraceId), UserResponse>.NewConfig()
  .Map(dest => dest.TraceId, src => src.TraceId)
  .Map(dest => dest, src => src.User);
```

var userResponse = (user, traceId).Adapt<UserResponse>();

Theres also BeforeMapping and AfterMapping methods

3 main types of configurations - 

1. Type mappings - one type to anotehr type
2. Global mappings
3. ForDestinationType

Lets say we have an interface called IValidatable - and it has a method called Validate. Lets say UserResponse implements this interface.

If we want to call this method whenever we initialize an object of this class, we can do - 
config.ForDestinationType<IValidatable>().AfterMapping(dest => dest.Validate())

TypeAdapterConfig.GlobalSettings.Default.MapToConstructor(true);

Additionally, instead of using the Adapt method, we can also do - 
IMapper mapper = new Mapper();
var userResponse = mapper.Map<UserResponse>(user);

CODE - 

Add Mapster and Mapster>DependencyInjection to API project
In the controller, any mapping that exists currently can be replaced with 
_mapper.Map<destinationClassName>(sourceObject)

In this scenario, RegisterRequest and RegisterCommand, we well as LoginRequest and LoginCommand
will work out of the box because they have the same properties.

AuthenticationResult to AuthenticationResult will need configuration
so we create APIProject/Common/Mapping/AuthenticationMappingConfig
Implement the IRegister interface

Because we want mapster to handle its own dependencyinjection, we create a file inside the same folder for this, called DependencyInjection

After configuring Mapster DI, we create another DI file for the presentation layer. We add the AddMappings method there, as well as move the other presentation layer related config like controllers into the presentation DI file and just called the Presentation layer DI method in Program.cs


### Chapter 8

#### Validation Behavior - FluentValidation

Mediator pipeline behaviors. We are going to validate a request in mediator before it reaches its corresponding handler

Create folder Application/Common/Behaviors and create ValidationBehaviors.cs. Add Mediator IpipelineBehavior classes
To wire it together, in Program.cs, add - 
```
        services.AddScoped<IPipelineBehavior<RegisterCommand, ErrorOr<AuthenticationResult>>, ValidateRegisterCommandBehavior>();
```

what happens is that before mediator invokes our handler, it wraps it in whatever class implements the IPipeline Behavior class where the type corresponds to the type of the request that it is currently executing 

Add FluentValidation to Application project

Because we are using Mediator to split our features (each feature sits in its own contained folder), we can just create Validators inside the folders

So we create (inside the Commands/Register folder). AbstractValidator is from FluentValidation

```
RegisterCommandValidator : AbstractValidator<RegisterCommand>
```

Add validation to this class. Then add it in DI 

 ```
         services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();
```

Or if we dont want to add each and every validator like this, install the FluentValidator aspnetcore package. Then you can replace the above line with - services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

Then we inject it into our ValidateRegisterCommandBehavior so we can use it like this - 
var validationResult = await _validator.ValidateAsync(request, cancellationToken);

If there were no validation errors, we invoke our handler using next()
If there were errors, then we have a list of errors in validationResult, and we are going to convert them to Errors using ErrorOr and then return them

#### Implementing a generic validation pipeline behavior
Next - we want to change the ValidationBehavior to work for any request type, and not just Register. So we change our class to use Generics

IPipelineBehavior<TRequest, TResponse>where TRequest is a mediator request and TResponse is whatever the request returns

If you have multiple validators for a particular request type, you can call all of them in the constructor and iterate through and invoke all of them in the Handle method

Then add dependency in DI -
```
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
```

### Chapter 9

#### JWT Bearer Authentication in ASP.NET 6

#### JWT

```json
// Header (algorithm and token type)
{ 
    "alg" : "H256",
    "typ" : "JWT",
    "cty" : "JWT"
}
```

```json
// Payload data
{ 
    "sub" : "SOME-GUID",
    "given-name" : "Nigel",
    "family-name" : "Gunawardene",
    "jti" : "SOME-GUID",
    "exp" : 111111223,
    "iss" : "BuberDinner",
    "aud" : "BuberDinner"
}
```

```json
// Signature
HMACSHA256(
$"{base64UrlEncode(header)}.{base64UrlEncode(payload)}","super-secret-key")
```

We generate a token when the user logs in. BuberDinner is the issuer and the audience. 
We use the HMACSHA256 algorithm to sign the token with the super secret key (which is exactly 16 bytes)

This will be called in the login endpoint, and will return a token in the response.

Generally, you will have 2 separate systems
One will be the identity provider - like AAD which will generate the token
Backend will have to get the public key and validate the token, but in this example, since we are the issuer and the system, we can use Asymmetric key

go to program.cs

Add -  app.UseAuthentication();

To make things simpler, in the Infrastructure DependencyInjection class - 
Create a new method called AddAuth, move auth lines to that, and call it in the AddInfrastructure method

Now we need to add the Dependencies that the authentication middleware needs, and we need to specify what we want to validate.

Add the Microsoft.AspNetCore.Authentication.JwtBearer package

AuthenticationScheme is simple the "Bearer" scheme

AddAuthentication adds the depencendies and also returns the AuthenticationBuilder, which internally has a map between the authentication scheme and the corresponding authentication handler
and what we want to do is add our Jwt Bearer Handler as the Authentication handler for the bearer authentication scheme

The actual authentication handler validates the token and makes sure its legit and populates the Identity of the User. We pass the params that we want to validate when configuring the services. 

#### Authorization

Now we have set up authentication, but to determine if an authenticated user is able to access an endpoint or not, we need to setup authorization - 

In the Presentation DependencyInjection, it already calls AddControllers, which calls AddAuthorization for us, so  we dont have to add the dependencies for that.

We need to do add - app.UseAuthorization(); in program.cs

In our pipeline, we hit the useAuthentication (authentication middleware) which finds the correct authentication handler which knows how to handle the bearer authentication scheme (JwtBearerHandler) and it gets whether or not the user is authenticated.

After that, we call the next middleware - Authorization middleware, which decides if the user can actually access the endpoitn

We add the [Authorization] attribute to our ApiController, so that it applies to all controllers that extend it, and we add [AllowAnonymous] to our AuthenticationController so that it can be accessed without authentication.

### Chapter 10

This was about 
Mapping Software Logic Using Process Modeling - https://www.youtube.com/watch?v=1pBGc7kKOAA&list=PLzYkqgWkHPKBcDIP5gzLfASkQyTdy0t4k&index=11

### Chapter 11

#### Modeling Domains

In this chapter we looked at modeling complex domains before implementing them in our application.
Link - https://www.youtube.com/watch?v=f6G46rqkePc
We want to have as many aggregates as possible. This is because each Aggregate is a TRANSACTIONAL BOUNDARY. 
Bigger aggregates means more work to be done if there is a change to be made. 
For example, in a menu, we have entities like Host, Dinner and Reviews. If a change is made, we would have to change the entire object. Instead of this, we can remove Host, Dinner and Reviews and replace them with hostId, dinnerId and reviewIds. (Host, Dinner and Reviews will be their own aggregate) In this way, aggregates can refer to each other. Aggregate root in this case would be Menu. 
When aggregates want to refer to one another, they do it by ID.
We create a value object which has a single property (the ID of the other aggregate) 

An aggregate is a collection of one or more related entities (and possibly value objects). Each aggregate has a single root entity, referred to as the aggregate root. The aggregate root is responsible for controlling access to all of the members of its aggregate.

### Chapter 12

#### Implementing AggregateRoot, Entity, ValueObject

In this chapter, we create a new folder in BuberDinner.Domain -> Common -> Models and in here we create the base classes for the different domain objects. Value objects for the aggregate root and for the entity. 


### Chapter 13

#### Domain Layer Structure & Skeleton


TRANSACTIONAL BOUNDARY 

A DDD aggregate is a cluster of domain objects that can be treated as a single unit. 
An example may be an order and its line-items, these will be separate objects, but it's useful to treat the order (together with its line items) as a single aggregate, because they conceptually belong together.

Aggregation encapsulates entity objects and value objects and takes the most important entity object as the aggregate root. As the only external portal of aggregation, the aggregate root ensures the consistency of business rules and data.

In this chapter, we start by creating the Menu Aggregate using the base classes that we created in chapter 11. (BubberDinner.Domain.Menu)

Aggregate rules - 

Reference other aggregates by ID
Changes are committed and rolled back as a whole
Changes to the aggregate are done through the root

Define each entity as an aggregate (dinner, bill, menu, etc)
Merge aggregates to enforce invariants (reservation entity moved into dinner entity)
Merge aggregates that cannot tolerate eventual consistency (domain events)

Data Model - Nothing special

Entity - Two entities are considered equal if they have the same ID

Value objects - Two are considered equal if they have the same value(s)


### Chapter 14

#### REST + DDD + CA + CQRS - When it all plays together

In this chapter, we create a Menu and understand which logic goes into which layer. 

Note that we are going to skip the logic where a USER becomes a HOST. 

##### PRESENTATION LAYER

BuberDinner.Contracts:
Menus - CreateMenuRequest and MenuResponse

BuberDinner.Api:
Common - Mapping - MenuMappingConfig
Common - Controllers - MenusController

##### APPLICATION LAYER

Menu - Commands - CreateMenu - CreateMenuCommand, CreateMenuCommandHandler, CreateMenuCommandValidator

Common - Interfaces - Persistence - IMenuRepository

##### INFRASTRUCTURE LAYER

Persistence - MenuRepository, DependencyInjection

#####  DOMAIN LAYER

MenuAggregate - Entities - MenuItem, MenuSection

MenuAggregate - ValueObjects - MenuId, MenuItemId, MenuSectionId

MenuReviewAggregate - MenuReview

MenuReviewAggregate - ValueObjects - MenuReviewId


##### Creating files

Create Docs/Api/Api.Menu.md and create the request/response models for documentation best practices
In BuberDinner.Contracts, create the Menu request and response, mirroring the documentation

In BuberDinner.Api, create our new controller

Then we create the corresponding Command for mediatr (from the request) which will  invoke the CreateMenuCommandHandler

In BuberDinner.Application, create Command and CommandHandler

Now we have 3 copies of the Menu - the MenuRequest in the ContractLayer, the MenuCommand in the ApplicationLayer and the Menu Aggregate in the DomainLayer. The reason for this is that in future as our application grows, we can allow each layer to grow and morph independently of the other layers. All we have to do is update the relevant layer and update the mapping with the other layers. 

After creating the handler, we work on the persistence layer.
Add the IMenuRepository to the ApplicationLayer and wire it up in the handler

Then implement the MenuRepository in the InfrastructureLayer

Then create the Validator





















































































































