using CSharpFunctionalExtensions;
using RentHall.Application.AbstractionsServices;
using RentHall.Core.AbstractionsRepositories.HallAbstractions;
using RentHall.Core.Models.AggregateHall;

namespace RentHall.Application.Services
{
    public class AdditionalServicesService: IAdditionalServicesServices
    {
        private IAdditionalServicesRepository _additionalServicesRepository;
        public AdditionalServicesService(IAdditionalServicesRepository additionalServicesRepository)
        {
            _additionalServicesRepository = additionalServicesRepository;
        }

        public Result<List<AdditionalServices>> GetAllServices()
        {
            try
            {
                return Result.Success(_additionalServicesRepository.GetAll().Value);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<AdditionalServices>>(ex.Message);
            }
        }

        public Result<List<AdditionalServices>> GetInHallServices(string name)
        {
            try
            {
                return Result.Success(_additionalServicesRepository.GetInHall(name).Value);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<AdditionalServices>>(ex.Message);
            }
        }

        public Result<Guid> CreateAdditionalServices(
            string name,
            decimal amount,
            string currency)
        {
            try
            {
                var newPrice = Price.Create(amount, currency).Value;
                var newServices = AdditionalServices.Create(Guid.NewGuid(), name, newPrice, DateTime.Now).Value;
                return Result.Success(_additionalServicesRepository.Create(newServices).Value);
            }
            catch (Exception ex)
            {
                return Result.Failure<Guid>(ex.Message);
            }
        }

        public Result UpdateAdditionalServices(
            Guid id,
            string name,
            decimal amount,
            string currency)
        {
            try
            {
                var newPrice = Price.Create(amount, currency).Value;
                var newServices = AdditionalServices.Create(Guid.NewGuid(), name, newPrice, DateTime.Now).Value;
                _additionalServicesRepository.Update(id, newServices);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public Result DeleteAdditionalServices(Guid id)
        {
            try
            {
                _additionalServicesRepository.Delete(id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
