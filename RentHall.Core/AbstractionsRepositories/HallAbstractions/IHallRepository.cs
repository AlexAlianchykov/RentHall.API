using CSharpFunctionalExtensions;
using RentHall.Core.Models.AggregateHall;

namespace RentHall.Core.AbstractionsRepositories.HallAbstractions
{
    public interface IHallRepository
    {
        public Result<List<Hall>> GetAll();
        public string GetNameHall(Guid id);
        public Result<Guid> Create(Hall hall, List<string>? additionalServices);
        public Result Update(Guid id, Hall hall, List<string>? addServices, List<string>? removeServices);
        public Result Delete(Guid id);

    }
}
