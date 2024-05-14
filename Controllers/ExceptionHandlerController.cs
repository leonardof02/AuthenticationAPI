using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiddleApi.Exceptions;

namespace MiddleApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ErrorsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        return Problem(
            title: "Unknown error",
            detail: "An unknown error occurred",
            statusCode: StatusCodes.Status500InternalServerError
        );
    }
}