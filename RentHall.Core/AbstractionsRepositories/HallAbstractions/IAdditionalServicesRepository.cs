using CSharpFunctionalExtensions;
using RentHall.Core.Models.AggregateHall;

namespace RentHall.Core.AbstractionsRepositories.HallAbstractions
{
    public interface IAdditionalServicesRepository
    {
        public Result<List<AdditionalServices>> GetAll();
        public Result<List<AdditionalServices>> GetInHall(string name);
        public Result<Guid> Create(AdditionalServices services);
        public Result Update(Guid id, AdditionalServices services);
        public Result Delete(Guid id);
    }
}
