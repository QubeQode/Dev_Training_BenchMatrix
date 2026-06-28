using ParkingManagementAPI.DTOs;
using ParkingManagementAPI.Models;

namespace ParkingManagementAPI.Exceptions;

public class InvalidFeeStrategyTypeException : ParkingLotException
{
    public InvalidFeeStrategyTypeException (FeeStrategyType feeStrategyType) 
        : base($"The input fee strategy {feeStrategyType} does not exist."){}
}
