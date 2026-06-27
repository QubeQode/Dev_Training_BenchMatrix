using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public interface IParkingFeeService
{
    decimal CalculateFeeAsync(ParkingTicket ticket);
}
