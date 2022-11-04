using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using RailwayReservationSystem.Models;

namespace RailwayReservationSystem.Data
{
    public class Transactions
    {
        [CreditCard(ErrorMessage ="Enter a valid Card Number")]
        public string CardNumber { get; set; }

        [Range(2022, 2050, ErrorMessage = "Must be between 2022 and 2050")]
        public int ExpiryYear { get; set; }

        [Range(1, 12, ErrorMessage = "Must be between 1 and 12")]
        public int ExpiryMonth { get; set; }

        //[MinLength(3, ErrorMessage ="Minimum Length is 3")]
        //[MaxLength(3, ErrorMessage = "Maximum Length is 3")]
        public int CVV { get; set; }
        public string CardHolderName { get; set; }

        
    }
}