using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RailwayReservationSystem.Models
{
    [Table("User")]
    public class User
    {
        public User()
        {
            Passenger = new HashSet<Passenger>();
            
        }

        public int UserId { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public ICollection<Passenger> Passenger { get; set; }
        public ICollection<Reservation> Reservation { get; set; }

    }
}