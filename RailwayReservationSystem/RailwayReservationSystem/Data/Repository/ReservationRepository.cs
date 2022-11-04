using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RailwayReservationSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Xml.Schema;

namespace RailwayReservationSystem.Data.Repository
{
    public class ReservationRepository : IReservationRepository
    {

        private readonly RailwayDbContext _context;
        private readonly ILogger<Reservation> _log;
        private readonly ITrainRepository _trainRepo;

        public ReservationRepository(RailwayDbContext context, ILogger<Reservation> log, ITrainRepository trainRepo)
        {
            _context = context;
            _log = log;
            _trainRepo = trainRepo;
        }

        

        #region "Add New Reservation"
        public Ticket AddReservation(ReservationDto reservation, int userId, List<Passenger> passengers)
        {
            List<Reservation> reservations = new List<Reservation>();
            List<PassengerTicket> passengerTickets = new List<PassengerTicket>();

            if (reservation != null && passengers.Count > 0)
            {
                try
                {
                    User user = _context.Users.Find(userId);
                    Train train = _context.Trains.Find(reservation.TrainId);
                    Random random = new Random();


                    //If Train Object is null
                    //Or
                    //Train Date is already gone then return NULL
                    if (train == null || DateTime.Now >= train.SourceDepartureTime) return null;
                    
                    int totalFare = (int)(passengers.Count * train.SeatFare);


                    //Getting the maximum transaction number from DB and add random number from 11-19 to it makes UNIQUE RANDOM Transaction ID
                    long transactionNumber;
                    try { transactionNumber = _context.Reservations.Max(res => res.TransactionNumber) + random.Next(11, 19); }
                    catch { transactionNumber = 798648310; }


                    //Same as Transaction Id Generation
                    int pnr;
                    try { pnr = _context.Reservations.Max(res => res.PnrNumber) + random.Next(11, 19); }
                    catch { pnr = 546781230; }


                    //Same as Transaction Id Generation
                    long seat;
                    try { seat = _context.Reservations.Max(res => res.seatNumber); }
                    catch { seat = 0; }



                    train = updateSeates(train, reservation.QuotaName, passengers.Count);

                    if (train == null) return null;

                    foreach (var pass in passengers)
                    {
                        ++seat;
                        Reservation reserve = new Reservation()
                        {
                            Train = train,
                            User = user,
                            Passenger = pass,
                            PnrNumber = pnr,
                            Status = "Confirmed",
                            TotalFare = (decimal)(totalFare * 1.00),
                            TransactionNumber = transactionNumber,
                            QuotaName = reservation.QuotaName,
                            seatNumber = seat,
                            BookingDate = DateTime.Now
                        };
                        passengerTickets.Add(new PassengerTicket { Name = pass.Name, Age = pass.Age, Gender = pass.Gender, Phone = pass.Phone, seat = reserve.seatNumber });

                        _context.Reservations.Add(reserve);
                        _context.SaveChanges();
                        reservations.Add(reserve);

                    }


                    Ticket ticket = new Ticket()
                    {
                        TrainNo = train.TrainNo,
                        TrainName = train.TrainName,
                        PnrNumber = pnr,
                        BookingDate = reservations[0].BookingDate,
                        SourceStation = train.SourceStation,
                        DestinationStation = train.DestinationStation,
                        SourceDepartureTime = train.SourceDepartureTime,
                        DestinationArrivalTime = train.DestinationArrivalTime,
                        TotalFare = (decimal)(totalFare * 1.00),
                        TransactionNumber = transactionNumber,
                        Status = reservations[0].Status,
                        QuotaName = reservations[0].QuotaName,
                        PassengerTicket = passengerTickets,
                    };

                    return ticket;
                }
                catch (Exception error)
                {
                    _log.LogError(error.Message);
                }

            }

            return null;
        }
        #endregion


        #region "Update Seates"
        private Train updateSeates(Train train, string quota, int seat)
        {
            switch (quota)
            {
                case "General":
                case "general":
                    if (train.AvailableGeneralSeat < seat) return null;
                    train.AvailableGeneralSeat -= seat;

                    break;
                case "Ladies":
                case "ladies":
                    if (train.AvailableLadiesSeat < seat) return null;
                    train.AvailableLadiesSeat -= seat;
                    break;
            }

            return train;
        }
        #endregion


        #region "Get Ticket"
        public Ticket GetTicket(int pnr, string role, int userId)
        {
            Ticket ticket = new Ticket();
            List<Reservation> reservations = new List<Reservation>();
            if (role == "Admin")
            {
                reservations = _context.Reservations.Where(reservation => reservation.PnrNumber == pnr).ToList();
            }
            else if(role == "User")
            {
                reservations = _context.Reservations.Where(reservation => reservation.PnrNumber == pnr && reservation.UserId == userId).ToList();
            }

            if (reservations.Count == 0)
            {
                return null;
            }
            List<PassengerTicket> passengers = new List<PassengerTicket>();
            
            foreach (var re in reservations)
            {
                List<Passenger> pass = _context.Passengers.Where(passenger => passenger.PassengerId == re.PassengerId).ToList();
                if(pass.Count > 0)
                    passengers.Add(new PassengerTicket { Name = pass[0].Name, Age = pass[0].Age, Gender = pass[0].Gender, Phone = pass[0].Phone, seat = re.seatNumber });
            }
            
            

            if (reservations.Count > 0)
            {
                Train train = _context.Trains.Find(reservations[0].TrainId);
                
                ticket.TrainNo = train.TrainNo;
                ticket.TrainName = train.TrainName;
                ticket.PnrNumber = reservations[0].PnrNumber;
                ticket.BookingDate = reservations[0].BookingDate;
                ticket.SourceStation = train.SourceStation;
                ticket.DestinationStation = train.DestinationStation;
                ticket.SourceDepartureTime = train.SourceDepartureTime;
                ticket.DestinationArrivalTime = train.DestinationArrivalTime;
                ticket.TotalFare = reservations[0].TotalFare;
                ticket.TransactionNumber = reservations[0].TransactionNumber;
                ticket.Status = reservations[0].Status;
                ticket.QuotaName = reservations[0].QuotaName;
                ticket.PassengerTicket = passengers;
            }

            return ticket;
        }
        #endregion


        #region "Cancel Ticket"
        public Ticket CancelTicket(int pnr)
        {
            List<Reservation> reservations = _context.Reservations.Where(reservation => reservation.PnrNumber == pnr && reservation.Status != "Cancelled").ToList();

            Train train = new Train();
            try { train = _context.Trains.Find(reservations[0].TrainId); }
            catch { return null; }


            //Reservation Only can be cancelled if the Train date is not the same day or past.
            if (DateTime.Now >= train.SourceDepartureTime)
            {
                return null;
            }

            foreach (Reservation reservation in reservations)
            {

                reservation.ReservationId = reservation.ReservationId;
                reservation.PassengerId = reservation.PassengerId;
                reservation.TrainId = reservation.TrainId;
                reservation.UserId = reservation.UserId;
                reservation.PnrNumber = reservation.PnrNumber;
                reservation.TransactionNumber = reservation.TransactionNumber;
                reservation.TotalFare = reservation.TotalFare;
                reservation.Status = "Cancelled";
                reservation.QuotaName = reservation.QuotaName;
                reservation.seatNumber = reservation.seatNumber;

                _context.Entry(reservation).State = EntityState.Modified;
                _context.SaveChanges();
            }


            switch (reservations[0].QuotaName)
            {
                case "General":
                case "general":
                    train.AvailableGeneralSeat += reservations.Count;

                    break;
                case "Ladies":
                case "ladies":

                    train.AvailableLadiesSeat += reservations.Count;
                    break;
            }

            _context.Entry(train).State = EntityState.Modified;
            _context.SaveChanges();

            return GetTicket(pnr, "Admin", 0);
        }
        #endregion
    }
}
