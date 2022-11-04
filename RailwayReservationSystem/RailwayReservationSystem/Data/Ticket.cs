using RailwayReservationSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace RailwayReservationSystem.Data
{
    public class Ticket
    {
        [Key]
        public int TrainNo { get; set; }
        public string TrainName { get; set; }
        
        public int PnrNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public string SourceStation { get; set; }
        public string DestinationStation { get; set; }
        public DateTime SourceDepartureTime { get; set; }
        public DateTime DestinationArrivalTime { get; set; }
        public decimal TotalFare { get; set; }
        public long TransactionNumber { get; set; }
        public string Status { get; set; }

        public string QuotaName { get; set; }
        
        public List<PassengerTicket> PassengerTicket { get; set; }
    }
}
