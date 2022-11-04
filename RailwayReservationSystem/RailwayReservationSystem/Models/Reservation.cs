using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RailwayReservationSystem.Models
{
    [Table("Reservation")]
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }
        public int TrainId { get; set; }
        public int UserId { get; set; }
        public int PassengerId { get; set; }
        public int PnrNumber { get; set; }
        public string Status { get; set; }
        public decimal TotalFare { get; set; }
        public long TransactionNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public string QuotaName { get; set; }
        public long seatNumber { get; set; }

        [ForeignKey("PassengerId")]
        public Passenger Passenger { get; set; }



        public Train Train { get; set; }

        public User User { get; set; }
    }
}