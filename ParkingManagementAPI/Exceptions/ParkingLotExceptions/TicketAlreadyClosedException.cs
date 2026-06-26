namespace ParkingManagementAPI.Exceptions;

public class TicketAlreadyClosedException : ParkingLotException
{
    public TicketAlreadyClosedException (int parkingTicketId) 
        : base($"Parking ticket {parkingTicketId} is already closed."){}
}
