using RailwayReservationSystem.Models;
using System.Collections.Generic;

namespace RailwayReservationSystem.Data.Repository
{
    public interface IPassengerRepository
    {
        List<Passenger> AddPassenger(List<PassengerDto> passengers, int userId);
    }
}
