using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RailwayReservationSystem.Models
{
    [Table("Train")]
    public class Train
    {
        public Train()
        {
            Reservation = new HashSet<Reservation>();
        }

        public int TrainId { get; set; }
        public int TrainNo { get; set; }
        public string TrainName { get; set; }
        public string SourceStation { get; set; }
        public string DestinationStation { get; set; }
        public DateTime SourceDepartureTime { get; set; }
        public DateTime DestinationArrivalTime { get; set; }
        public int TotalSeat { get; set; }
        public int AvailableGeneralSeat { get; set; }
        public int AvailableLadiesSeat { get; set; }
        public decimal SeatFare { get; set; }
        public ICollection<Reservation> Reservation { get; set; }
    }
}