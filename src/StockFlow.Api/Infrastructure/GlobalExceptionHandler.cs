using FluentValidation;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using StockFlow.Domain.Exceptions;

namespace StockFlow.Api.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        if (exception is ValidationException validationException)
        {
            problemDetails.Title = "Validation Failed";
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
            problemDetails.Detail = "One or more validation errors occurred.";

            problemDetails.Extensions["errors"] = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
        }

        else if (exception is KeyNotFoundException)
        {
            problemDetails.Title = "Resource Not Found";
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
            problemDetails.Detail = exception.Message;
        }

        else if (exception is DomainException domainException)
        {
            problemDetails.Title = domainException.Title;
            problemDetails.Status = StatusCodes.Status409Conflict;
            problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8";
            problemDetails.Detail = domainException.Message;

            if (exception is InsufficientStockException stockEx)
            {
                problemDetails.Extensions["currentStock"] = stockEx.CurrentStock;
                problemDetails.Extensions["requestedQuantity"] = stockEx.RequestedQuantity;
            }
        }

        else
        {
            return false;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
