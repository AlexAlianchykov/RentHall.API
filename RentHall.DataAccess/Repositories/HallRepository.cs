using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using RentHall.Core.AbstractionsRepositories.HallAbstractions;
using RentHall.Core.Models.AggregateHall;
using RentHall.DataAccess.Entities;

namespace RentHall.DataAccess.Repositories
{
    public class HallRepository: IHallRepository
    {
        private ApplicationDbContext _dbContext;
        public HallRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public Result<List<Hall>> GetAll()
        {
            var halls = _dbContext.Halls
                                .AsNoTracking()
                                .ToList();

            List<Hall> newList = new List<Hall>();  
            foreach (var hall in halls)
            {
                var newPrice = Price.Create(
                    hall.Amount, 
                    hall.Currency).Value;

                var newHall = Hall.Create(
                    hall.Id,
                    hall.Name,
                    hall.Сapacity,
                    newPrice,
                    hall.CreateDate).Value;
                newList.Add(newHall);
            }

            return Result.Success(newList);
        }
        public string GetNameHall(Guid id)
        {
            var hall = _dbContext.Halls.FirstOrDefault(x => x.Id == id);
            return (hall.Name);
        }
        
        public Result<Guid> Create(Hall hall, List<string>? additionalServices)
        {
            var hallInDb = _dbContext.Halls.FirstOrDefault(a => a.Name == hall.Name);

            if (hallInDb != null)
            {
                return Result.Failure<Guid>("A hall with this name already exists");
            }

            HallEntity hallEntity = new()
            {
                Id = hall.Id,
                Name = hall.Name,
                Сapacity = hall.Сapacity,
                Amount = hall.Price.Amount,
                Currency = hall.Price.Currency,
                CreateDate = hall.CreateDate
            };
            _dbContext.Halls.Add(hallEntity);

            if(additionalServices != null)
            {
                foreach (var services in additionalServices)
                {
                    var service = _dbContext
                        .Additionals
                        .AsNoTracking()
                        .FirstOrDefault(x => x.Name.ToUpper() == services.ToUpper());

                    if (service == null)
                    {
                        return Result.Failure<Guid>($"Services {services.ToUpper()} is not in the general list of services");
                    }

                    HallAdditionalServicesEntity hallAdditionalServicesEntity = new()
                    {
                        HallID = hall.Id,
                        AdditionalServicesID = service.Id
                    };
                    _dbContext.HallsAdditionals.Add(hallAdditionalServicesEntity);
                }
            }

            _dbContext.SaveChanges();
            return Result.Success(hallEntity.Id);
        }

        public Result Update(Guid id, Hall hall, List<string>? addServices, List<string>? removeServices)
        {
            var copyName = _dbContext.Halls.FirstOrDefault(y => y.Name == hall.Name && y.Id == id);

            if (copyName == null)
            {
                var hallIdInDb = _dbContext.Halls.FirstOrDefault(y => y.Id == id);
                if (hallIdInDb != null)
                {
                    var hallNameInDb = _dbContext.Halls.FirstOrDefault(y => y.Name == hall.Name);
                    if (hallNameInDb != null)
                    {
                        return Result.Failure("A hall with this name already exists");
                    }

                     _dbContext.Halls
                               .Where(x => x.Id == id)
                               .ExecuteUpdate(s => s
                               .SetProperty(x => x.Name, hall.Name)
                               .SetProperty(x => x.Сapacity, hall.Сapacity)
                               .SetProperty(x => x.Amount, hall.Price.Amount)
                               .SetProperty(x => x.Currency, hall.Price.Currency));

                }
                else
                {
                    return Result.Failure("There is no hall with this ID");
                }
            }
            else
            {
                _dbContext.Halls
                          .Where(x => x.Id == id)
                          .ExecuteUpdate(s => s
                          .SetProperty(x => x.Сapacity, hall.Сapacity)
                          .SetProperty(x => x.Amount, hall.Price.Amount)
                          .SetProperty(x => x.Currency, hall.Price.Currency));
            }
            
            if (addServices != null)
            {
                foreach (var services in addServices)
                {
                    var service = _dbContext
                        .Additionals
                        .FirstOrDefault(x => x.Name.ToUpper() == services.ToUpper());

                    if (service == null)
                    {
                        return Result.Failure($"Services {services.ToUpper()} is not in the general list of services");
                    }

                    var hallServiceInDb = _dbContext
                                            .HallsAdditionals
                                            .FirstOrDefault(x => x.HallID == id 
                                                              && x.AdditionalServicesID == service.Id);

                    if (hallServiceInDb == null)
                    {
                        HallAdditionalServicesEntity hallAdditionalServicesEntity = new()
                        {
                            HallID = id,
                            AdditionalServicesID = service.Id
                        };
                        _dbContext.HallsAdditionals.Add(hallAdditionalServicesEntity);
                    }
                }
            }

            if (removeServices != null)
            {
                foreach (var services in removeServices)
                {
                    var service = _dbContext
                        .Additionals
                        .FirstOrDefault(x => x.Name.ToUpper() == services.ToUpper());

                    if (service == null)
                    {
                        return Result.Failure($"Services {services.ToUpper()} is not in the general list of services");
                    }

                    _dbContext
                            .HallsAdditionals
                            .Where(x => x.HallID == id
                                     && x.AdditionalServicesID == service.Id)
                            .ExecuteDelete();
                }
            }

            _dbContext.SaveChanges();
            return Result.Success();

        }

        public Result Delete(Guid id)
        {
            var hallEntity = _dbContext.Halls.FirstOrDefault(x => x.Id == id);

            if (hallEntity == null)
            {
                return Result.Failure("There is no hall with such ID");
            }

            _dbContext.Halls
                .Where(x => x.Id == id)
                .ExecuteDelete();

            _dbContext.HallsAdditionals
                  .Where(y => y.HallID == id)
                  .ExecuteDelete();

            _dbContext.Bookings
                 .AsNoTracking()
                 .Where(x => x.HallID == id)
                 .ExecuteDelete();

            return Result.Success();
        }
    }
}
