using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FluentValidationException = FluentValidation.ValidationException;

namespace DealmateApi.Service.Exceptions;

public class HttpResponseExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        var errorResponse = new
        {
            message = exception.Message,
            stackTrace = exception.StackTrace
        };
        if (exception is BadRequestException badRequestEx)
        {
            context.Result = new ObjectResult(new { error = errorResponse })
            {
                StatusCode = badRequestEx.StatusCode
            };
            context.ExceptionHandled = true;
        }
        else if (exception is ConflictException conflictEx)
        {
            context.Result = new ObjectResult(new { error = errorResponse })
            {
                StatusCode = conflictEx.StatusCode
            };
            context.ExceptionHandled = true;
        }
        else if (exception is FluentValidationException valEx)
        {
            var combinedErrors = string.Join(", ",valEx.Errors
                .Select(e => e.ErrorMessage.TrimEnd('.')).Distinct());

            context.Result = new ObjectResult(new
            {
                message = "Validation failed.",
                errors = combinedErrors
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest // Use appropriate status code
            };
            context.ExceptionHandled = true;
        }
        else
        {
            context.Result = new ObjectResult(new { error = errorResponse })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
            context.ExceptionHandled = true;
        }
    }
}
