using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Route("Train")]
    public class TrainController : ControllerBase
    {
        private readonly ITrainRepository _repo;
        private readonly EmailRepository _email;

        public TrainController(ITrainRepository repo, EmailRepository email)
        {
            _repo = repo;
            _email = email;
        }

        #region "SearchTrain"
        [HttpPost]
        [Route("Search")]
        public IActionResult SearchTrain([FromBody] SearchTrain search)
        {

            if (ModelState.IsValid)
            {
                if (search.TrainDate < DateTime.Now.Date)
                {
                    return BadRequest(new { msg = "You cannot search past trains" });
                }
                List<Train> trains = _repo.SearchTrainDetails(search);
                if (trains.Count > 0)
                {
                    return Ok(trains);
                }
                return NotFound(new { msg = "No train for this Search..." });
            }

            return ValidationProblem("Fill the data Properly...");
        }
        #endregion

        #region "UpdateTrain"
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route(("Update"))]
        public IActionResult UpdateTrain([FromBody] TrainDtoput trainDtoput)
        {
            if (ModelState.IsValid)
            {
                //if (trainDtoput.AvailableGeneralSeat + trainDtoput.AvailableLadiesSeat != trainDtoput.TotalSeat)
                //{
                //    return BadRequest(new { msg = "Total of General and Ladies seats must be same as Total Seats" });
                //}
                if (trainDtoput.SourceDepartureTime > trainDtoput.DestinationArrivalTime)
                {
                    return BadRequest(new { msg = "Source Departure Time should be before Destination Arrival Time" });
                }
                if (trainDtoput.SourceDepartureTime <= DateTime.Now)
                {
                    return BadRequest(new { msg = "Source Departure Time should be Future Date" });
                }
                if (_repo.IsSeatsUpdated(trainDtoput))
                {
                    return BadRequest(new { msg = "Seat Updation is Disabled ...." });
                }
                Train train = _repo.UpdateTrainDetails(trainDtoput);
                
                if (train == null)
                {
                    return NotFound(new { msg = "No Train Found...." });
                }

                return Ok(train);
            }

            return ValidationProblem("Fill the data Properly...");
        }
        #endregion

        #region "AddTrain"
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Add")]
        public ActionResult<Train> AddTrain([FromBody] TrainDto trainDto)
        {
            if (ModelState.IsValid)
            {
                if(trainDto.SourceStation == trainDto.DestinationStation)
                {
                    return BadRequest(new { msg = "Source Station and Sestination Station must be Different" });
                }
                if (trainDto.AvailableGeneralSeat + trainDto.AvailableLadiesSeat != trainDto.TotalSeat)
                {
                    return BadRequest(new { msg = "Total of General and Ladies seats must be same as Total Seats" });
                }
                if(trainDto.SourceDepartureTime > trainDto.DestinationArrivalTime)
                {
                    return BadRequest(new { msg = "Source Departure Time should be before Destination Arrival Time" });
                }
                if (trainDto.SourceDepartureTime <= DateTime.Now)
                {
                    return BadRequest(new { msg = "Source Departure Time should be Future Date" });
                }

                Train train = _repo.AddTrainDetails(trainDto);
                if (train == null)
                    return Conflict(new { msg = "Some error happens...Try Again" });

                return CreatedAtAction("AddTrain", train);
            }

            return ValidationProblem("Fill the data Properly...");
        }
        #endregion

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("View")]
        public ActionResult GetTrains()
        {
            List<Train> train = _repo.GetAllTrains();
            if(train.Count > 0)
            {
                return Ok(train);
            }
            return NotFound(new { msg = "No Trains..." });
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("View/{id}")]
        public ActionResult GetTrain(int id)
        {
            var train =  _repo.GetTrain(id);

            if (train == null)
            {
                return NotFound(new { msg = "Train Not available for this Search" });
            }

            return Ok(train);
        }
    }
}
