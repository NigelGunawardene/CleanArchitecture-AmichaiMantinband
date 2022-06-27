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

dotnet run -- project .\BuberDinner.Api\

---

Install Rest Client and create folders Requests/something.http

    



























