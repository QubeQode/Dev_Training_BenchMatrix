namespace ParkingManagementAPI.Exceptions;

public class TicketNotFoundException : ParkingLotException
{
    public TicketNotFoundException(int parkingTicketId) 
        : base($"Parking Ticket {parkingTicketId} was not found."){}
}
