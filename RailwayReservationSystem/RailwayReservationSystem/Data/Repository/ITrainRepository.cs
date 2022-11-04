using Microsoft.EntityFrameworkCore;
using RailwayReservationSystem.Models;
using System.Collections.Generic;

namespace RailwayReservationSystem.Data.Repository
{
    public interface ITrainRepository
    {
        List<Train> SearchTrainDetails(SearchTrain dto);
        Train SearchTrainDetailsById(int id);

        Train UpdateTrainDetails(TrainDtoput dto);
        Train AddTrainDetails(TrainDto dto);
        bool IsSeatsUpdated(TrainDtoput dtoPut);
        bool TrainExists(int id);

        public List<Train> GetAllTrains();

        public Train GetTrain(int id);

        bool CheckAvailability(int trainId, string quota, int passCount);
    }
}
