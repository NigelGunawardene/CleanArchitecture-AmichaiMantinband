using BuberDinner.Api.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ApiController]// any class that extends this controller will get this attribute
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        // we have access to HttpContext because we are in a controller
        HttpContext.Items[HttpContextItemKeys.Errors] = errors; //we assign this here so that in our ErrorFactory, we can access it to set a custom property (BuberDinnerProblemDetailsFactory)

        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}
