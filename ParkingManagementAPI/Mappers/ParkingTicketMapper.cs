using ParkingManagementAPI.Models;
using ParkingManagementAPI.DTOs;
using ParkingManagementAPI.Exceptions;

namespace ParkingManagementAPI.Mappers;

public static class ParkingMapper
{
    public static Vehicle ToVehicle(ParkVehicleRequestDTO dto)
    {
        return dto.VehicleType switch
        {
            VehicleType.Car => new Car
            {
                LicensePlate = dto.LicensePlate,
                Make = dto.Make,
                Model = dto.Model,
                PassengerIsHandicapped = dto.PassengerIsHandicapped
            },

            VehicleType.Motorcycle => new Motorcycle
            {
                LicensePlate = dto.LicensePlate,
                Make = dto.Make,
                Model = dto.Model,
                PassengerIsHandicapped = dto.PassengerIsHandicapped
            },

            VehicleType.Truck => new Truck
            {
                LicensePlate = dto.LicensePlate,
                Make = dto.Make,
                Model = dto.Model,
                PassengerIsHandicapped = dto.PassengerIsHandicapped
            },

            _ => throw new InvalidVehicleTypeException(dto.VehicleType)
        };
    }

    public static ParkingTicketResponseDTO ToResponse(ParkingTicket ticket)
    {
        return new ParkingTicketResponseDTO
        {
            TicketId = ticket.ParkingTicketId,
            FloorNumber = ticket.ParkingSpot.Floor.FloorNumber,
            ParkingSpotName = ticket.ParkingSpot.ParkingSpotName,
            TimeOfIssuance = ticket.TimeOfIssuance,
            TimeOfConclusion = ticket.TimeOfConclusion,
            TotalFee = ticket.TotalFee,
            FeeStrategyType = ticket.FeeStrategyType,
            IsOpen = ticket.IsOpen
        };
    }
}
