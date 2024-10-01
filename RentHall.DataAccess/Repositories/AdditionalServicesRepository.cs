using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using RentHall.Core.AbstractionsRepositories.HallAbstractions;
using RentHall.Core.Models.AggregateHall;
using RentHall.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace RentHall.DataAccess.Repositories
{
    public class AdditionalServicesRepository: IAdditionalServicesRepository
    {
        private ApplicationDbContext _dbContext;
        public AdditionalServicesRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public Result<List<AdditionalServices>> GetAll()
        {
            var allServices = _dbContext.Additionals.AsNoTracking().ToList();

            List<AdditionalServices> additionalServices = new List<AdditionalServices>();

            foreach (var service in allServices)
            {
                var newPrice = Price.Create(
                    service.Amount,
                    service.Currency);

                var newAdditionalServices = AdditionalServices.Create(
                    service.Id,
                    service.Name,
                    newPrice.Value,
                    service.CreateDate
                    ).Value;
                additionalServices.Add( newAdditionalServices );
            }

            return Result.Success( additionalServices );
        }
        
        public Result<List<AdditionalServices>> GetInHall(string name)
        {
            var hall = _dbContext.Halls.FirstOrDefault(x => x.Name == name);

            if (hall == null)
            {
               return Result.Failure<List<AdditionalServices>>("There is no hall with this name");
            }

            var servicesInHall = _dbContext.HallsAdditionals
                                        .Where(y => y.HallID == hall.Id)
                                        .ToList();

            List<AdditionalServices> newList = new List<AdditionalServices>();

            foreach (var service in servicesInHall)
            {
                var serviceHall = _dbContext.Additionals.FirstOrDefault(a => a.Id == service.AdditionalServicesID);

                var newPrice = Price.Create(serviceHall.Amount, serviceHall.Currency).Value;

                var newService = AdditionalServices.Create(
                    serviceHall.Id,
                    serviceHall.Name,
                    newPrice,
                    serviceHall.CreateDate).Value;
                newList.Add(newService);
            }
            return Result.Success(newList);
        }

        public Result<Guid> Create(AdditionalServices services)
        {
            var service = _dbContext
                            .Additionals
                            .FirstOrDefault(x => x.Name == services.Name
                                              && x.Amount == services.Price.Amount
                                              && x.Currency == services.Price.Currency);

            if (service != null)
            {
                return Result.Failure<Guid>("Such a service already exists");
            }

            var copyName = _dbContext.Additionals.FirstOrDefault(y => y.Name == services.Name);

            if (copyName != null)
            {
                return Result.Failure<Guid>($"A service named {services.Name.ToUpper()} already exists.");
            }

            AdditionalServicesEntity additionalServicesEntity = new()
            {
                Id = services.Id,
                Name = services.Name,
                Amount = services.Price.Amount,
                Currency = services.Price.Currency,
                CreateDate = services.CreateDate
            };
            _dbContext.Additionals.Add(additionalServicesEntity);
            _dbContext.SaveChanges();

            return Result.Success(additionalServicesEntity.Id);
        }

        public Result Update(Guid id, AdditionalServices services)
        {
            var copyName = _dbContext.Additionals.FirstOrDefault(y => y.Name == services.Name && y.Id == id);

            if (copyName == null)
            {
                var serviceIdInDb = _dbContext.Additionals.FirstOrDefault(x => x.Id == id);
                if (serviceIdInDb != null)
                {
                    var serviceNameInDb = _dbContext.Additionals.FirstOrDefault(y => y.Name == services.Name);

                    if (serviceNameInDb != null)
                    {
                        return Result.Failure("A service with this name already exists");
                    }

                    _dbContext.Additionals
                      .Where(x => x.Id == id)
                      .ExecuteUpdate(s => s
                      .SetProperty(x => x.Name, services.Name)
                      .SetProperty(x => x.Amount, services.Price.Amount)
                      .SetProperty(x => x.Currency, services.Price.Currency));

                }
                else
                {
                    return Result.Failure("There is no service with this identifier.");
                }

            }
            else
            {
                _dbContext.Additionals
                      .Where(x => x.Id == id)
                      .ExecuteUpdate(s => s
                      .SetProperty(x => x.Amount, services.Price.Amount)
                      .SetProperty(x => x.Currency, services.Price.Currency));
            }

            _dbContext.SaveChanges();
            return Result.Success();
        }
        
        public Result Delete(Guid id)
        {
            var serviceEntity = _dbContext.Additionals.FirstOrDefault(x => x.Id == id);

            if (serviceEntity == null)
            {
                return Result.Failure("There is no service with this ID");
            }

            _dbContext.Additionals
                .Where(x => x.Id == id)
                .ExecuteDelete();

            _dbContext.HallsAdditionals
              .AsNoTracking()
              .Where(y => y.AdditionalServicesID == id)
              .ExecuteDelete();

            _dbContext.BookingsAdditionals
                .AsNoTracking()
                .Where(x => x.AdditionalServicesID == id)
                .ExecuteDelete();

            return Result.Success();
        }

    }
}
