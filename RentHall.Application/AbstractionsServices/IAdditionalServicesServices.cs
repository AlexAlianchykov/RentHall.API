using CSharpFunctionalExtensions;
using RentHall.Core.Models.AggregateHall;

namespace RentHall.Application.AbstractionsServices
{
    public interface IAdditionalServicesServices
    {
        public Result<List<AdditionalServices>> GetAllServices();

        public Result<List<AdditionalServices>> GetInHallServices(string name);

        public Result<Guid> CreateAdditionalServices(
            string name,
            decimal amount,
            string currency);

        public Result UpdateAdditionalServices(
            Guid id,
            string name,
            decimal amount,
            string currency);

        public Result DeleteAdditionalServices(Guid id);
    }
}
