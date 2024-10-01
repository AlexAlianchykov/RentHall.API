using CSharpFunctionalExtensions;
using RentHall.Core.Models.AggregateHall;

namespace RentHall.Application.AbstractionsServices
{
    public interface IHallService
    {
        public Result<List<Hall>> GetAllHall();

        public string GetNameHall(Guid id);

        public Result<Guid> CreateHall(
            string name,
            int capacity,
            decimal amount,
            string currency,
            List<string>? additionalServices);

        public Result UpdateHall(
            Guid id,
            string name,
            int capacity,
            decimal amount,
            string currency,
            List<string>? addServices,
            List<string>? removeServices);

        public Result DeleteHall(Guid id);
    }
}
