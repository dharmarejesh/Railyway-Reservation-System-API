using RailwayReservationSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservationSystem.Data
{
    public class ReservationDto
    {
        public int TrainId { get; set; }
        public string QuotaName { get; set; }
        public List<PassengerDto> Passengers { get; set; }
        public Transactions Transaction { get; set; }
    }
}
