using BusinessLayer.Extentions;
using BusinessLayer.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BrokerAPI.Middleware;

/// <summary>
///     Middleware handle for exceptions
/// </summary>
public class AppMiddlewareException
{
    private readonly IWebHostEnvironment _environment;
    private readonly RequestDelegate _next;

    public AppMiddlewareException(RequestDelegate next, IWebHostEnvironment environment)
    {
        _next = next;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }

        catch (AggregateException exp)
        {
            await HandleExceptionAsync(context, exp.GetBaseException(), HttpStatusCode.InternalServerError);
        }
        catch (ArgumentNullException exp)
        {
            await HandleExceptionAsync(context, exp, HttpStatusCode.BadRequest);
        }
        catch (ArgumentException exp)
        {
            await HandleExceptionAsync(context, exp, HttpStatusCode.BadRequest);
        }
        catch (NotSupportedException exp)
        {
            await HandleExceptionAsync(context, exp, HttpStatusCode.BadRequest);
        }
        catch (InvalidOperationException exp)

        {
            await HandleExceptionAsync(context, exp, HttpStatusCode.BadRequest);
        }
        catch (Exception exp)
        {
            await HandleExceptionAsync(context, exp, HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    ///     Create error response
    /// </summary>
    private async Task HandleExceptionAsync(HttpContext context, Exception exp, HttpStatusCode code)
    {
        var resultObject = new ProblemDetails
        {
            Status = (int)code,
            Title = exp.Message,
            Instance = context.Request.Path,
            Type = code.ToString()
        };

        if (!_environment.IsProduction())
            resultObject.Detail = exp.FullMessage();

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        await context.Response.WriteAsync(resultObject.SerializeToString());
    }
}


