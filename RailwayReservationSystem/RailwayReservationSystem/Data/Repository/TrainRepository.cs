using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RailwayReservationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RailwayReservationSystem.Data.Repository
{
    public class TrainRepository : ITrainRepository
    {
        RailwayDbContext _context;
        private readonly ILogger<Train> _log;

        public TrainRepository(RailwayDbContext context, ILogger<Train> log)
        {
            _context = context;
            _log = log;
        }
        #region "Add Train Details"
        public Train AddTrainDetails(TrainDto trainDto)
        {
            try
            {
                var train = new Train()
                {
                    TrainNo = trainDto.TrainNo,
                    TrainName = trainDto.TrainName,
                    SourceStation = trainDto.SourceStation,
                    DestinationStation = trainDto.DestinationStation,
                    SourceDepartureTime = trainDto.SourceDepartureTime,
                    DestinationArrivalTime = trainDto.DestinationArrivalTime,
                    TotalSeat = trainDto.TotalSeat,
                    AvailableGeneralSeat = trainDto.AvailableGeneralSeat,
                    AvailableLadiesSeat = trainDto.AvailableLadiesSeat,
                    SeatFare = trainDto.SeatFare

                };
                _context.Trains.Add(train);
                _context.SaveChanges();
                return train;
            }catch(Exception error)
            {
                _log.LogError(error.Message);
            }
            return null;
        }
        #endregion

        #region "Search Train Details"
        public List<Train> SearchTrainDetails(SearchTrain dto)
        {
            List<Train> trainList = _context.Trains.Where(t => t.SourceStation == dto.From && t.DestinationStation == dto.To && t.SourceDepartureTime.Date == dto.TrainDate.Date).ToList();
            return trainList;
        }
        #endregion

        #region "Srarch Train Setails By Id"
        public Train SearchTrainDetailsById(int id)
        {
            try
            {
                return _context.Trains.FirstOrDefault(tr => tr.TrainId == id);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region "Train Exists Method"
        public bool TrainExists(int id)
        {
            return _context.Trains.Any(e => e.TrainId == id);
        }
        #endregion

        #region "Update Train Details"
        public Train UpdateTrainDetails(TrainDtoput trainDtoput)
        {

            Train train = new Train();
            try { train = _context.Trains.FirstOrDefault(tr => tr.TrainId == trainDtoput.TrainId); }
            catch { return null; }

            train.TrainId = trainDtoput.TrainId;
            train.TrainNo = trainDtoput.TrainNo;
            train.TrainName = trainDtoput.TrainName;
            train.SourceStation = trainDtoput.SourceStation;
            train.DestinationStation = trainDtoput.DestinationStation;
            train.SourceDepartureTime = trainDtoput.SourceDepartureTime;
            train.DestinationArrivalTime = trainDtoput.DestinationArrivalTime;
            train.TotalSeat = train.TotalSeat;
            train.AvailableGeneralSeat = train.AvailableGeneralSeat;
            train.AvailableLadiesSeat = train.AvailableLadiesSeat;
            train.SeatFare = trainDtoput.SeatFare;

            _context.Entry(train).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
                return train;
            }
            catch (Exception error)
            {
                _log.LogError(error.Message);
            }
            return null;
        }

        public bool IsSeatsUpdated(TrainDtoput dtoPut)
        {
            Train train = new Train();
            try { train = _context.Trains.FirstOrDefault(tr => tr.TrainId == dtoPut.TrainId); }
            catch { return true; }
            if (train.TotalSeat != dtoPut.TotalSeat)
            {
                return true;
            }
            return false;
        }
        #endregion

        public List<Train> GetAllTrains()
        {
            
            return _context.Trains.Where(tr => tr.SourceDepartureTime >= DateTime.Now).OrderBy(tr => tr.SourceDepartureTime).ToList();
        }

        public Train GetTrain(int id)
        {
            try
            {
                return _context.Trains.FirstOrDefault(tr => tr.TrainId == id && tr.SourceDepartureTime >= DateTime.Now);
            }
            catch
            {
                return null;
            }
        }

        public bool CheckAvailability(int trainId, string quota, int passCount)
        {
            Train train = GetTrain(trainId);
            if(train == null)
            {
                return false;
            }
            if(quota == "General")
            {
                if(train.AvailableGeneralSeat >= passCount)
                {
                    return true;
                }
                
            }
            else if(quota == "Ladies")
            {
                if(train.AvailableLadiesSeat >= passCount)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
