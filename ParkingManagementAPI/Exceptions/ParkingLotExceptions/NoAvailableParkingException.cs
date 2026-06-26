namespace ParkingManagementAPI.Exceptions;

public class NoAvailableParkingException : ParkingLotException
{
    public NoAvailableParkingException () 
        : base("There are no available parking spots."){}
}
