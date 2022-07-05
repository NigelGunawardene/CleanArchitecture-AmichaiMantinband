using BuberDinner.Api.Filters;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Api.Controllers;

//[ApiController] // since this attribute will be on all controllers that extend from our ApiController class, we can just move this annotation to the ApiController class
[Route("auth")]
//[ErrorHandlingFilter]
public class AuthenticationController : ApiController
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

        // ErrorOr library

        return registerResult.Match(
   authResult => Ok(MapAuthResult(authResult)),
        errors => Problem(errors) // pass this to the method defined in the ApiController class
    );

        //return registerResult.MatchFirst(
        //    authResult => Ok(MapAuthResult(authResult)),
        //    firstError => Problem(statusCode: StatusCodes.Status409Conflict, title: firstError.Description)
        //    );

        // FluentResults Library --------------------------------------------------------------------
        //if (registerResult.IsSuccess)
        //{
        //    return Ok(MapAuthResult(registerResult.Value));
        //}

        //var firstError = registerResult.Errors[0];
        //if (firstError is DuplicateEmailError)
        //{
        //    return Problem(statusCode: StatusCodes.Status409Conflict, title: "Email already exists - thrown from FluentResults DuplicateEmailError");
        //}

        //return Problem();


        // OneOf Library --------------------------------------------------------------------
        // when using OneOf, we can use this method, since there are only 2 possible responses.
        //return registerResult.Match(
        //    authResult => Ok(MapAuthResult(authResult)),
        //    error => Problem(statusCode: (int)error.StatusCode, title: error.ErrorMessage));

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


        //
        if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: authResult.FirstError.Description);
        }


        return authResult.Match(
   authResult => Ok(MapAuthResult(authResult)),
        errors => Problem(errors) // pass this to the method defined in the ApiController class
        );


        //var response = new AuthenticationResponse(authResult.User.Id, authResult.User.FirstName, authResult.User.LastName, authResult.User.Email, authResult.Token);
        //return Ok(response);
    }



    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(authResult.User.Id, authResult.User.FirstName, authResult.User.LastName, authResult.User.Email, authResult.Token);
    }

}
