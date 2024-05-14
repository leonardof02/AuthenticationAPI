using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MiddleApi.Exceptions;

namespace MiddleApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {

        if (context.Exception is HttpResponseException httpResponseException)
        {
            var problems = new ProblemDetails
            {
                Status = httpResponseException.StatusCode,
                Title = context.Exception.Message
            };
            context.Result = new ObjectResult(problems);
            context.ExceptionHandled = true;
        }
    }
}