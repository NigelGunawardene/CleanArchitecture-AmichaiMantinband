using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[Route("auth")]
[AllowAnonymous]
public class AuthenticationController : ApiController
{
    // normally this would be fine. But since we practice I in SOLID, interface segregation principle, we dont use IMediator, but use ISender/IPublisher
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AuthenticationController(ISender mediator, IMapper mapper)
    {
        _sender = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        // Instead of manually mapping this, we can use Mapster
        //var command = new RegisterCommand(request.FirstName, request.LastName, request.Email, request.Password);
        var command = _mapper.Map<RegisterCommand>(request);
        ErrorOr<AuthenticationResult> authResult = await _sender.Send(command);

        return authResult.Match(authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)), errors => Problem(errors)); // pass this to the method defined in the ApiController class
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);
        var authResult = await _sender.Send(query);

        if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: authResult.FirstError.Description);
        }

        return authResult.Match(authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)), errors => Problem(errors)); // pass this to the method defined in the ApiController class
    }

    // This entire method is not needed anymore because we are using Mapster
    //private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    //{
    //    return new AuthenticationResponse(authResult.User.Id, authResult.User.FirstName, authResult.User.LastName, authResult.User.Email, authResult.Token);
    //}
}
