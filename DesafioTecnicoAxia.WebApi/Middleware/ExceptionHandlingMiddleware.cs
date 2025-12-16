using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using DesafioTecnicoAxia.Application.Common.Exceptions;

namespace DesafioTecnicoAxia.WebApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case NotFoundException notFoundException:
                code = HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(new { error = notFoundException.Message });
                break;
            case KeyNotFoundException keyNotFoundException:
                code = HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(new { error = keyNotFoundException.Message });
                break;
                        case ArgumentException argumentException:
                            code = HttpStatusCode.BadRequest;
                            result = JsonSerializer.Serialize(new { error = argumentException.Message });
                            break;
                        default:
                            _logger.LogError(exception, "Erro não tratado: {Message}. StackTrace: {StackTrace}", 
                                exception.Message, exception.StackTrace);
                            
                            var isDevelopment = context.RequestServices.GetService<IHostEnvironment>()?.IsDevelopment() == true;
                
                if (isDevelopment)
                {
                    var fullError = exception.Message;
                    if (exception.InnerException != null)
                    {
                        fullError += $"\n\nInner Exception: {exception.InnerException.Message}";
                        fullError += $"\n\nInner StackTrace: {exception.InnerException.StackTrace}";
                    }
                    fullError += $"\n\nStackTrace: {exception.StackTrace}";
                    result = JsonSerializer.Serialize(new { error = fullError });
                }
                else
                {
                    result = JsonSerializer.Serialize(new { error = "Ocorreu um erro interno no servidor." });
                }
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}

