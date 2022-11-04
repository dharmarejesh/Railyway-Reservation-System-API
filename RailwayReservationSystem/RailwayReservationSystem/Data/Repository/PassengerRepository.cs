using Microsoft.Extensions.Logging;
using RailwayReservationSystem.Models;
using System;
using System.Collections.Generic;

namespace RailwayReservationSystem.Data.Repository
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly RailwayDbContext _context;
        private readonly ILogger<Passenger> _log;

        public PassengerRepository(RailwayDbContext context, ILogger<Passenger> log)
        {
            _context = context;
            _log = log;
        }

        #region "Add Passenger"
        public List<Passenger> AddPassenger(List<PassengerDto> passengers, int userId)
        {
            List<Passenger> passes = new List<Passenger>();
            try
            {
                User user = _context.Users.Find(userId);
                if(user == null)
                {
                    return null;
                }
                foreach (var pass in passengers)
                {
                    Passenger passenger = new Passenger()
                    {
                        UserId = userId,
                        User = user,
                        Gender = pass.Gender,
                        Name = pass.Name,
                        Age = pass.Age,
                        Phone = pass.Phone
                    };
                    _context.Passengers.Add(passenger);
                    _context.SaveChanges();
                    passes.Add(passenger);
                }
            }
            catch(Exception error)
            {
                _log.LogError(error.Message);
                return new List<Passenger>();
            }
            
            return passes;
            
        }
        #endregion
    }
}
