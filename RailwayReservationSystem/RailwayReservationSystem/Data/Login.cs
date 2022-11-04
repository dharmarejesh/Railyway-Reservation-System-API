using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using RailwayReservationSystem.Models;

namespace RailwayReservationSystem.Data
{
    public class Login
    {
        [Required(ErrorMessage = "UserId is Required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
    }
}