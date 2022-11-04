using System;
using System.ComponentModel.DataAnnotations;

namespace RailwayReservationSystem.Data
{
    public class TrainDtoput : TrainDto
    {
        [Required(ErrorMessage = " TrainId is Mandotory Field")]
        public int TrainId { get; set; }
        
    }
}
