using System;
using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
                                    IHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            // we are just moving on if everything is okay
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = environment.IsDevelopment()
                ? new APIException(httpContext.Response.StatusCode, ex.Message, ex.StackTrace)
                : new APIException(httpContext.Response.StatusCode, ex.Message, "Internal Server Error");
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);

            await httpContext.Response.WriteAsync(json);
        }
    }
}
