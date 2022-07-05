using BuberDinner.Api.Filters;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ApiController]
[Route("auth")]
//[ErrorHandlingFilter]
public class AuthenticationController : ControllerBase
{

    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var registerResult = _authenticationService.Register(request.FirstName, request.LastName, request.Email, request.Password);

        return registerResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            _ => Problem(statusCode: StatusCodes.Status409Conflict, title: "Email already exists - thrown from AuthenticationController"));

        // the code below can be done using the match method above ^
        //if (registerResult.IsT0)
        //{
        //    var authResult = registerResult.AsT0;
        //    var response = MapAuthResult(authResult);
        //    return Ok(response);
        //}
        //return Problem(statusCode: StatusCodes.Status409Conflict, title: "Email already exists - thrown from AuthenticationController");
    }


    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var authResult = _authenticationService.Login(request.Email, request.Password);
        var response = new AuthenticationResponse(authResult.User.Id, authResult.User.FirstName, authResult.User.LastName, authResult.User.Email, authResult.Token);
        return Ok(response);
    }
    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(authResult.User.Id, authResult.User.FirstName, authResult.User.LastName, authResult.User.Email, authResult.Token);
    }

}
