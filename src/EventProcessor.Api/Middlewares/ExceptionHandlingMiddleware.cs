using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace EventProcessor.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Serilog.ILogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, Serilog.ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(httpContext, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        _logger.Error(exception, exception.Message);

        var errorMessage = exception.Message;
        var response = exception switch
        {
            UnauthorizedAccessException _ => new
            {
                ErrorMessage = errorMessage,
                ErrorCode = (int)HttpStatusCode.Unauthorized
            },
            ValidationException _ => new
            {
                ErrorMessage = errorMessage,
                ErrorCode = (int)HttpStatusCode.BadRequest
            },
            DbUpdateException _ => new
            {
                ErrorMessage = "Database error. Please, retry later",
                ErrorCode = (int)HttpStatusCode.InternalServerError
            },
            _ => new
            {
                ErrorMessage = "Internal server error. Please, retry later",
                ErrorCode = (int)HttpStatusCode.InternalServerError
            }
        };

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)response.ErrorCode;
        await httpContext.Response.WriteAsJsonAsync(response);
    }
}