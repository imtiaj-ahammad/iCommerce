using System.Net;
using Newtonsoft.Json;

namespace Product.Command.API;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

        var errorResponse = new
            {
                message = "An error occurred while processing your request.",
                details = exception.Message
            };

        var jsonErrorResponse = JsonConvert.SerializeObject(errorResponse);

        await context.Response.WriteAsync(jsonErrorResponse);
    }    
}
