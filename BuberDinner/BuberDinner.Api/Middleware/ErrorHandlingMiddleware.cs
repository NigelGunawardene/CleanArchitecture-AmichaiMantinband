using System.Net;
using Newtonsoft.Json;

namespace BuberDinner.Api.Middleware;


// this is the middleware approach to error handling. we will now disable it from the program.cs
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = JsonConvert.SerializeObject(new { error = exception.Message });
        context.Response.ContentType = "appliation/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}
