using CSharpFunctionalExtensions;
using RentHall.Core.Models.AggregateHall;

namespace RentHall.Application.AbstractionsServices
{
    public interface IBookingService
    {
        public Result<List<Hall>> GetFreeHalls(
            DateTime start,
            int duration,
            int capacity);

        public Result<decimal> CreateBookingHall(
            string token,
            DateTime start,
            int duration,
            Guid hallId,
            List<string>? services);

        public Result DeleteBooking(Guid id);
    }
}
