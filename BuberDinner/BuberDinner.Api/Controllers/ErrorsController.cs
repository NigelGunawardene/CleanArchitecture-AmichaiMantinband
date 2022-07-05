using BuberDinner.Application.Common.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        var (statusCode, message) = exception switch
        {
            //DuplicateEmailException _ => (StatusCodes.Status409Conflict, "Email already exists from flow control - DuplicateEmailException"), // this is a security risk. dont confirm that the email exists
            //_ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")

            IServiceException serviceException => ((int)serviceException.StatusCode, serviceException.ErrorMessage),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };


        return Problem(statusCode: statusCode, title: message); // can add title: exception.Message, statusCode: 400
    }
}
