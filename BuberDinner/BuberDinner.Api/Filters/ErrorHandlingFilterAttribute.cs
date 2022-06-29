using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BuberDinner.Api.Filters;

// to use this we add an attribute [ErrorHandlingFilter] to the controller or add to program.cs
public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        // this class matches the RFC problem details specification
        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "An error occured while processing your request",
            Status = (int)HttpStatusCode.InternalServerError,
            Instance = context.HttpContext.Request.Path,
            Detail = exception.Message
        };

        //var errorResult = new { error = "An error occured while processing your request" };

        context.Result = new ObjectResult(problemDetails);
        //{
        //    StatusCode = 500
        //};

        context.ExceptionHandled = true;
    }
}
