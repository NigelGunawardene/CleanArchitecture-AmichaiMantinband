using BuberDinner.Api.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BuberDinner.Api.Controllers;

[ApiController]// any class that extends this controller will get this attribute
[Authorize]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }
        // fixing the error message from validation
        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        // we have access to HttpContext because we are in a controller
        HttpContext.Items[HttpContextItemKeys.Errors] = errors; //we assign this here so that in our ErrorFactory, we can access it to set a custom property (BuberDinnerProblemDetailsFactory)

        var firstError = errors[0];
        return Problem(firstError);
    }

    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
        return Problem(statusCode: statusCode, title: error.Description);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        // create model state dictionary

        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(modelStateDictionary);// comes from controllerbase. accepts a model state dictionary.
    }
}
