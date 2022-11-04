using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RailwayReservationSystem.Data;
using RailwayReservationSystem.Data.Repository;
using RailwayReservationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RailwayReservationSystem.Controllers
{
    [Route("Book")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _resRepo;
        private readonly IPassengerRepository _passRepo;
        private readonly ITrainRepository _trainRepo;
        private readonly EmailRepository _mail;
        private readonly ILogger<Reservation> _log;

        public ReservationController(IReservationRepository resRepo, IPassengerRepository passRepo, ITrainRepository trainRepo, EmailRepository mail, ILogger<Reservation> log)
        {
            _resRepo = resRepo;
            _passRepo = passRepo;
            _trainRepo = trainRepo;
            _mail = mail;
            _log = log;
        }

        #region "Reservation & Payment"
        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("Pay")]
        public async Task<IActionResult> BookNowAsync([FromBody] ReservationDto dto)
        {
            var userClaims = HttpContext.User.Claims;

            Ticket ticket = null;
            if (ModelState.IsValid)
            {
                if (dto.Passengers.Count > 6)
                {
                    return BadRequest(new { msg = "Maximum 6 Passenger allowed in 1 booking..." });
                }
                if (!_trainRepo.CheckAvailability(dto.TrainId, dto.QuotaName, dto.Passengers.Count)){
                    return BadRequest(new { msg = "Seats for " + dto.Passengers.Count + " Passengers Not Available in " + dto.QuotaName + " Quota" });
                }

                //Getting the user details from JWT while user is loged In
                int userId = 0;
                if (userClaims == null) return BadRequest(new { msg = "Please Login Again..." });

                try { userId = Convert.ToInt32(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier).Value); }
                catch { return null; }


                try
                {
                    List<Passenger> passes = _passRepo.AddPassenger(dto.Passengers, userId);

                    if (passes.Count > 0)
                    {
                        ticket = _resRepo.AddReservation(dto, userId, passes);
                    }
                    else
                    {
                        return BadRequest(new { mag = "Please Fill The Reservation Form Properly..." });
                    }
                }
                catch (Exception error)
                {
                    _log.LogError(error.Message);
                    return BadRequest(error.Message);
                }
            }

            if (ticket != null)
            {
                //Processing Mailing Details
                string email = "";
                try { email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email).Value; }
                catch { _log.LogError("Email Not Found Of User..."); }

                if(email != null)
                {
                    MailDetails mailDetails = new MailDetails
                    {
                        To = email,
                        Subject = "Ticket"
                    };

                    await _mail.SendEmail(mail: mailDetails, pnr: ticket.PnrNumber);

                }
                return Ok(ticket);
            }

            return ValidationProblem("Fill the data Properly...");
        }
        #endregion


        #region "Get Ticket"
        [Authorize(Roles = "User, Admin")]
        [HttpGet("GetTicket/{pnr}")]
        public ActionResult GetReservation(int pnr)
        {
            var claims = HttpContext.User.Claims;
            string role;
            int userId = 0;
            try { 
                role = claims.FirstOrDefault(o => o.Type == ClaimTypes.Role).Value; 
                userId = Convert.ToInt32(claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier).Value); 
            }
            catch(Exception error) { 
               _log.LogError(error.Message);
                return BadRequest(new { msg= "User Not Valid..." });
            }

            Ticket ticket = null;
            if (role == "Admin")
            {
                ticket = _resRepo.GetTicket(pnr, role, 0);
            }
            else if(role == "User")
            {
                ticket = _resRepo.GetTicket(pnr, role, userId);
            }
            

            if (ticket == null)
            {
                return NotFound(new { msg = "Please Enter a Valid PNR Number...." });
            }
            return Ok(ticket);
        }
        #endregion


        #region "Cancel Ticket"
        [Authorize(Roles = "User")]
        [HttpPut]
        [Route("Cancel/{pnr}")]
        public async Task<ActionResult> CancelTicketAsync(int pnr)
        {
            Ticket ticket = _resRepo.CancelTicket(pnr);



            if (ticket == null)
            {
                return NotFound(new { msg = "Cancellation Failed....Check Your PNR" });
            }

            //Getting Logged in User Details
            var claim = HttpContext.User.Claims;
            string email = "";
            try { email = claim.FirstOrDefault(o => o.Type == ClaimTypes.Email).Value; }
            catch { _log.LogError("Email Not Found Of User..."); }

            if (email != null)
            {
                MailDetails mailDetails = new MailDetails
                {
                    To = email,
                    Subject = "Ticket Cancelled"
                };

                await _mail.SendEmail(mail: mailDetails, pnr: ticket.PnrNumber, cancelled: true);
            }
            return Ok(ticket);
        }
        #endregion
    }
}
