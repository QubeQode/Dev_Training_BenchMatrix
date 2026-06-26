using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ParkingManagementAPI.Exceptions;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync
    (
        HttpContext httpContext, 
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var (statusCode, title) = exception switch
        {
            ParkingLotNotFoundException =>
                (StatusCodes.Status404NotFound, "Parking Lot Not Found"),
            
            FloorNotFoundException =>
                (StatusCodes.Status404NotFound, "Floor Not Found"),
            
            ParkingSpotNotFoundException =>
                (StatusCodes.Status404NotFound, "Parking Spot Not Found"),
            
            TicketNotFoundException =>
                (StatusCodes.Status404NotFound, "Parking Ticket Not Found"),
            
            VehicleNotFoundException =>
                (StatusCodes.Status404NotFound, "Vehicle Not Found"),
            
            ParkingOccupiedException =>
                (StatusCodes.Status409Conflict, "Parking Already Occupied"),
            
            ParkingFreeException =>
                (StatusCodes.Status409Conflict, "Parking Already Free"),
            
            NoAvailableParkingException =>
                (StatusCodes.Status409Conflict, "No Available Parking"),
            
            TicketAlreadyClosedException =>
                (StatusCodes.Status409Conflict, "Ticket Already Closed"),
            
            VehicleAlreadyParkedException =>
                (StatusCodes.Status409Conflict, "Vehicle Already Parked"),
            
            VehicleDoesNotFitException =>
                (StatusCodes.Status400BadRequest, "Vehicle Does Not Fit Parking Spot"),
            
            _ =>
                (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken
        );

        return true;
    }
}