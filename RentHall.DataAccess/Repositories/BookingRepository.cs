using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using RentHall.Core.AbstractionsRepositories.HallAbstractions;
using RentHall.Core.Models.AggregateHall;
using RentHall.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RentHall.DataAccess.Repositories
{
    public class BookingRepository: IBookingRepository
    {
        private ApplicationDbContext _dbContext;
        public BookingRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public Result<decimal> Create(Booking booking, List<string>? services)
        {
            var hall = _dbContext.Halls.FirstOrDefault(x => x.Id == booking.HallID); // проверяем есть ли указанный зал 
            if (hall == null)
            {
                return Result.Failure<decimal>("There is no hall with this ID");
            }

            decimal totalPriceServices = 0;
            // проверка услуг
            if (services != null)   
            {
                // есть ли у зала доступ к выбранным услугам 


                var hallServices = _dbContext.HallsAdditionals    // услуги доступные залу
                                        .AsNoTracking()
                                        .Where(a => a.HallID == booking.HallID)
                                        .ToList();
                if (hallServices == null)
                {
                    return Result.Failure<decimal>("The hall does not have access to the required services");
                }

                if (services.Count > hallServices.Count)
                {
                    return Result.Failure<decimal>("Number of services required > number of services available");
                }

                bool serviceHall = false;
                foreach (var listServices in services)        
                {
                    var newServiceHall = _dbContext.Additionals.FirstOrDefault(a => a.Name.ToUpper() == listServices.ToUpper());

                    if (newServiceHall == null)
                    {
                        return Result.Failure<decimal>($"There is no service named {listServices.ToUpper()}");
                    }

                    totalPriceServices += newServiceHall.Amount;

                    foreach (var service in hallServices)  // есть ли выбранная услуга у конкретного зала 
                    {
                        if (newServiceHall.Id == service.AdditionalServicesID)
                        {
                            serviceHall = true;
                        }
                    }

                    if (serviceHall == false)
                    {
                        return Result.Failure<decimal>("The selected hall does not have such a service");
                    }

                    else   // проверяем забронирована ли данная услуга 
                    {
                        var bookingThisService = _dbContext.BookingsAdditionals
                                                                .AsNoTracking()
                                                                .Where(f => f.AdditionalServicesID == newServiceHall.Id)
                                                                .ToList();
                        if (bookingThisService != null)
                        {
                            foreach (var entity in bookingThisService)
                            {
                                var entityBooking = _dbContext.Bookings.FirstOrDefault(s => s.Id == entity.BookingID);

                                if (entityBooking.StartBooking.Month == booking.StartBooking.Month)
                                {
                                    if (entityBooking.StartBooking.Day == booking.StartBooking.Day)
                                    {
                                        if (entityBooking.StartBooking.Hour >= booking.StartBooking.Hour)
                                        {
                                            if (entityBooking.StartBooking.Hour >= (booking.StartBooking.Hour + booking.Duration))
                                            {
                                                
                                            }
                                            else
                                            {
                                                return Result.Failure<decimal>($"{newServiceHall.Name.ToUpper()} service is not available");
                                            }
                                        }
                                        else
                                        {
                                            if ((entityBooking.StartBooking.Hour + entityBooking.Duration) <= booking.StartBooking.Hour)
                                            {
                                               
                                            }
                                            else
                                            {
                                                return Result.Failure<decimal>($"{newServiceHall.Name.ToUpper()} service is not available");
                                            }
                                        }
                                    }
                                }

                            }
                        }

                        BookingAdditionalServicesEntity bookingAdditional = new()
                        {
                            BookingID = booking.Id,
                            AdditionalServicesID = newServiceHall.Id
                        };
                        _dbContext.BookingsAdditionals.Add(bookingAdditional);
                    }
                    serviceHall = false;
                }
            }



            var bookingList  = _dbContext.Bookings
                                    .AsNoTracking()
                                    .Where(y => y.HallID == booking.HallID)
                                    .ToList();

            // проверка зала 
            if (bookingList != null) // свободен или занят зал в выбранную дату
            {
                foreach (var entity in bookingList)
                {
                    if (entity.StartBooking.Month == booking.StartBooking.Month)
                    {
                        if (entity.StartBooking.Day == booking.StartBooking.Day)
                        {
                            if (entity.StartBooking.Hour >= booking.StartBooking.Hour)
                            {
                                if (entity.StartBooking.Hour >= (booking.StartBooking.Hour + booking.Duration))
                                {
                                    
                                }
                                else
                                {
                                    return Result.Failure<decimal>("The hall is already booked for this time");
                                }
                            }
                            else
                            {
                                if ((entity.StartBooking.Hour + entity.Duration) <= booking.StartBooking.Hour)
                                {
                                    
                                }
                                else
                                {
                                    return Result.Failure<decimal>("The hall is already booked for this time");
                                }
                            }
                        }
                    }
                    
                }

            }


            // расчёт стоимости бронирования 
            decimal totalPriceHall = 0;

            for (int i = 0; i < booking.Duration; ++i)
            {
                int currentHour = booking.StartBooking.Hour + i;

                decimal currentPrice = hall.Amount;

                if (currentHour >= 6 && currentHour < 9)
                {
                    currentPrice *= 0.9m;  // 10% скидка
                }
                else if (currentHour >= 12 && currentHour < 14)
                {
                    currentPrice *= 1.15m;  // 15% наценка
                }
                else if (currentHour >= 18 && currentHour < 23)
                {
                    currentPrice *= 0.8m;  // 20% скидка
                }
                totalPriceHall += currentPrice;
            }
            decimal totalPrice = totalPriceServices + totalPriceHall;

            BookingEntity bookingEntity = new()
            {
                Id = booking.Id,
                StartBooking = booking.StartBooking,
                Duration = booking.Duration,
                Amount = totalPrice,
                Currency = "UAH",
                CreateDate = booking.CreateDate,
                HallID = booking.HallID,
                UserID = booking.UserID

            };
            _dbContext.Bookings.Add(bookingEntity);
            _dbContext.SaveChanges();

            return Result.Success(totalPrice);
        }

        public Result<List<Hall>> GetFree(DateTime start, int duration, int capacity)
        {

            DateTime endBooking = start.AddHours(duration);

            // Получаем список всех залов, вместимость которых больше или равна количеству человек
            var availableHalls = _dbContext.Halls
                .Where(h => h.Сapacity >= capacity)
                .Where(h => !h.Bookings.Any(b =>
                    // Условие пересечения бронирований
                    (b.StartBooking < endBooking && b.StartBooking.AddHours(b.Duration) > start)))
                .ToList();

            List<Hall> newList = new List<Hall>();
            foreach (var hall in availableHalls)
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

        public Result Delete(Guid id)
        {
            var booking = _dbContext.Bookings.FirstOrDefault(x => x.Id == id);

            if (booking == null)
            {
                return Result.Failure("There is no booking with this ID");
            }

            _dbContext.Bookings
                .Where(x => x.Id == id)
                .ExecuteDelete();

            _dbContext.BookingsAdditionals
                .Where(y => y.BookingID == id)
                .ExecuteDelete();

            return Result.Success();
        }
    }
}
