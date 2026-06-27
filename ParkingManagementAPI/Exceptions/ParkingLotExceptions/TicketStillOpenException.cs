namespace ParkingManagementAPI.Exceptions;

public class TicketStillOpenException : ParkingLotException
{
    public TicketStillOpenException (int parkingTicketId) 
        : base($"Parking ticket {parkingTicketId} is still open."){}
}