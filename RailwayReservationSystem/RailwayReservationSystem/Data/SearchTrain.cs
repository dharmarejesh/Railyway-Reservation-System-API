using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using RailwayReservationSystem.Models;

namespace RailwayReservationSystem.Data
{
    public class SearchTrain
    {
        [Required (ErrorMessage ="From is Required")]
        public string From { get; set; }
        [Required(ErrorMessage = "To is Required")]
        public string To { get; set; }
        [Required(ErrorMessage = "TrainDate is Required")]
        [DataType(DataType.Date)]
        public DateTime TrainDate { get; set; }
    }
}