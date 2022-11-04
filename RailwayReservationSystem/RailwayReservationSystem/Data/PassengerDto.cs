using RailwayReservationSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservationSystem.Data
{
    public class PassengerDto
    {
        public string Gender { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        [Phone]
        public string Phone { get; set; }
        
    }
}
