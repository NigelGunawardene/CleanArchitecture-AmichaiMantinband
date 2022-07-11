using BuberDinner.Api.Filters;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Services.Authentication.Commands;
using BuberDinner.Application.Services.Authentication.Queries;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Application.Services.Authentication.Common;

namespace BuberDinner.Api.Controllers;

//[ApiController] // since this attribute will be on all controllers that extend from our ApiController class, we can just move this annotation to the ApiController class
[Route("auth")]
//[ErrorHandlingFilter]
public class AuthenticationController : ApiController
{

    private readonly IAuthenticationCommandService _authenticationCommandService;
    private readonly IAuthenticationQueryService _authenticationQueryService;

    public AuthenticationController(IAuthenticationCommandService authenticationService, IAuthenticationQueryService authenticationQueryService)
    {
        _authenticationCommandService = authenticationService;
        _authenticationQueryService = authenticationQueryService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var registerResult = _authenticationCommandService.Register(request.FirstName, request.LastName, request.Email, request.Password);

        return registerResult.Match(authResult => Ok(MapAuthResult(authResult)), errors => Problem(errors)); // pass this to the method defined in the ApiController class
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var authResult = _authenticationQueryService.Login(request.Email, request.Password);

        if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: authResult.FirstError.Description);
        }

        return authResult.Match(authResult => Ok(MapAuthResult(authResult)), errors => Problem(errors)); // pass this to the method defined in the ApiController class
    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(authResult.User.Id, authResult.User.FirstName, authResult.User.LastName, authResult.User.Email, authResult.Token);
    }

}
