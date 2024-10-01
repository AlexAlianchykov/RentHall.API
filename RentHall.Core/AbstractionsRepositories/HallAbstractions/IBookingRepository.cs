using CSharpFunctionalExtensions;
using RentHall.Core.Models.AggregateHall;

namespace RentHall.Core.AbstractionsRepositories.HallAbstractions
{
    public interface IBookingRepository
    {
        public Result<decimal> Create(Booking booking, List<string>? services);
        public Result<List<Hall>> GetFree(DateTime start, int duration, int capacity);
        public Result Delete(Guid id);
    }
}
