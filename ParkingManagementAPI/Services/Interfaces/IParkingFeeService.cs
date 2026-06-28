using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Services;

public interface IParkingFeeService
{
    Task<decimal> CalculateFeeAsync(ParkingTicket ticket, DateTime exitTime);
}
