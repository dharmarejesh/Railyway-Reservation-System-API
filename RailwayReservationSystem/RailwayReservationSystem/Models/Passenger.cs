using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RailwayReservationSystem.Models
{
    [Table("Passenger")]
    public class Passenger
    {
        [Key]
        public int PassengerId { get; set; }
        public int UserId { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        [Phone]
        public string Phone { get; set; }

        public Reservation Reservation { get; set; }
        public User User { get; set; }
    }
}