using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiddleApi.Exceptions;

namespace MiddleApi.Controllers;

[ApiController]
public class ErrorController : ControllerBase
{
    [Route("/error")]
    public IActionResult HandleError()
    {
        var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature?.Error;

        if (exception is HttpResponseException httpResponseException)
        {
            return Problem(
                detail: httpResponseException?.Message,
                statusCode: httpResponseException?.StatusCode,
                title: httpResponseException?.Source
            );
        }
        
        return Problem(
            detail: "An unhandled exception has occurred",
            statusCode: StatusCodes.Status500InternalServerError,
            title: "Unknown problem"
        );


    }
}