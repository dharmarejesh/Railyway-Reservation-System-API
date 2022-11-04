using System;
using System.ComponentModel.DataAnnotations;

namespace RailwayReservationSystem.Data
{
    public class TrainDto
    {
        [Required(ErrorMessage = " TrainNo is a Mandoatory Field")]
        public int TrainNo { get; set; }
        [Required(ErrorMessage = " TrainName is a Mandoatory Field")]
        public string TrainName { get; set; }
        [Required(ErrorMessage = " SourceStation is a Mandoatory Field")]
        public string SourceStation { get; set; }
        [Required(ErrorMessage = " is a Mandoatory Field")]
        public string DestinationStation { get; set; }
        [Required(ErrorMessage = " SourceDepartureTime is a Mandoatory Field")]
        public DateTime SourceDepartureTime { get; set; } 
        [Required(ErrorMessage = " DestinationArrivalTime is a Mandoatory Field")]
        public DateTime DestinationArrivalTime { get; set; }
        [Required(ErrorMessage = " TotalSeat is a Mandoatory Field")]
        public int TotalSeat { get; set; }
        [Required(ErrorMessage = " AvailableGeneralSeat is a Mandoatory Field")]
        public int AvailableGeneralSeat { get; set; }
        [Required(ErrorMessage = " AvailableLadiesSeat is a Mandoatory Field")]
        public int AvailableLadiesSeat { get; set; }
        [Required(ErrorMessage = " SeatFare is a Mandoatory Field")]
        public decimal SeatFare { get; set; }
    }
}
