using CSharpFunctionalExtensions;
using RentHall.Application.AbstractionsServices;
using RentHall.Core.AbstractionsRepositories.HallAbstractions;
using RentHall.Core.Models.AggregateHall;
using RentHall.Core.Models.AggregateUser;

namespace RentHall.Application.Services
{
    public class HallService: IHallService
    {
        private IHallRepository _hallRepository;
        public HallService(IHallRepository hallRepository)
        {
            _hallRepository = hallRepository;
        }

        public Result<List<Hall>> GetAllHall()
        {
            try
            {
                return Result.Success(_hallRepository.GetAll().Value);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Hall>>(ex.Message);
            }
        }

        public string GetNameHall(Guid id)
        {
            return (_hallRepository.GetNameHall(id));
        }

        public Result<Guid> CreateHall(
            string name,
            int capacity,
            decimal amount,
            string currency,
            List<string>? additionalServices)
        {
            try
            {
                var newPrice = Price.Create(amount, currency).Value;
                var newHall = Hall.Create(Guid.NewGuid(), name, capacity, newPrice, DateTime.Now).Value;

                return Result.Success(_hallRepository.Create(newHall, additionalServices).Value);
            }
            catch (Exception ex)
            {
                return Result.Failure<Guid>(ex.Message);
            }
        }

        public Result UpdateHall(
            Guid id,
            string name,
            int capacity,
            decimal amount,
            string currency,
            List<string>? addServices,
            List<string>? removeServices)
        {
            try
            {
                var newPrice = Price.Create(amount, currency).Value;
                var newHall = Hall.Create(Guid.NewGuid(), name, capacity, newPrice, DateTime.Now).Value;
                _hallRepository.Update(id, newHall, addServices, removeServices);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public Result DeleteHall(Guid id)
        {
            try
            {
                _hallRepository.Delete(id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
