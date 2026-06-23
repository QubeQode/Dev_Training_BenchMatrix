/*
    - When the internal error bubbles up through the middleware chain execution stops
    - Without exception handler catching error it is a 500 Internal Server Error
    - ExceptionHandler is the first link in the chain and acts as a try/catch around everything
    - CustomExceptionHandler is a translator that turns Exception --> Error + Problem Details

*/

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Exceptions;

public class CustomExceptionHandler : IExceptionHandler
{
    // ASP.NET IExceptionHandler excpects ValueTask and not Task on TryHandleAsync
    // Avoids creating Task object when a result is immediately available - optimization decision
    public async ValueTask<bool> TryHandleAsync
    (
        HttpContext httpContext, // Represents the current request and response
        Exception exception, // Actual exception that was thrown
        CancellationToken cancellationToken // Cancels work if the client disconnects or connection hangs
    )
    {
        // Filter to check if the exception is BookNotFound
        if (exception is BookNotFoundException)
        {
            // Translate the exception into 404 status - resource not found
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            // Standardized error format in Microsoft
            var problemDetails = new ProblemDetails
            {
                Title = "Book Not Found",
                Detail = exception.Message, // "Book with ID {id} was not found." - defined in BookNotFound
                Status = StatusCodes.Status404NotFound
            };

            // Async process to send back response of error data and status as JSON
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        return false;
    }
}
