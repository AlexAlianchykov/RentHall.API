using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentHall.API.Contracts;
using RentHall.Application.AbstractionsServices;
using RentHall.Application.Services;
using RentHall.Core.Models.AggregateHall;
using System.Collections.Generic;
using System.Security.Claims;

namespace RentHall.API.Controllers
{
    [Authorize(Policy = "UserPolisy" )]
    [Route("api/UserController")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IBookingService _bookingService;
        private IAdditionalServicesServices _additionalServicesServices;
        private IHallService _hallService;
        public UserController(
            IUserService userService,
            IBookingService bookingService,
            IAdditionalServicesServices additionalServicesServices,
            IHallService hallServise)
        {
            _userService = userService;
            _bookingService = bookingService;
            _additionalServicesServices = additionalServicesServices;
            _hallService = hallServise;
        }

        [HttpPost("FreeHalls")]   // поиск доступных залов
        public ActionResult<List<HallDtoRespons>> FreeHalls([FromBody] GetFreeHallsDto dto)
        {
            try
            {
                var listHall = _bookingService.GetFreeHalls(dto.Start, dto.Duration, dto.Capacity).Value;
                List<HallDtoRespons> newList = new List<HallDtoRespons>();
                foreach (var hall in listHall)
                {
                    var listServices = _additionalServicesServices.GetInHallServices(hall.Name).Value;
                    List<string> nameServise = new List<string>();
                    foreach (var service in listServices)
                    {
                        nameServise.Add(service.Name);
                    }
                    HallDtoRespons newHall = new()
                    {
                        Id = hall.Id,
                        Name = hall.Name,
                        Capacity = hall.Сapacity,
                        Amount = hall.Price.Amount,
                        Currency = hall.Price.Currency,
                        additionalServices = nameServise
                    };
                    newList.Add(newHall);
                }

                return Ok(newList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ServicesInHall")] // список дополнительных услуг в определённом зале 
        public ActionResult<List<AdditionalServicesDto>> GetServicesInHall(string name)
        {
            try
            {
                var listAdditServ = _additionalServicesServices.GetInHallServices(name).Value;
                List<AdditionalServicesDto> newList = new List<AdditionalServicesDto>();
                foreach (var service in listAdditServ)
                {
                    AdditionalServicesDto additionalServicesDto = new()
                    {
                        Name = service.Name,
                        Amount = service.Price.Amount,
                        Currency = service.Price.Currency
                    };
                    newList.Add(additionalServicesDto);
                }

                return Ok(newList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateBooking")]     // забронить зал
        public ActionResult<decimal> CreateNewBooking([FromBody] BookingDtoRequest dto)
        {
            try
            {
                var token = HttpContext.Request.Cookies["ttt"]; 

                return Ok(_bookingService.CreateBookingHall( token, dto.Start, dto.Duration, dto.HallId, dto.services).Value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ListBookingUser")]   // посмотреть список бронирований 
        public ActionResult<List<BookingDtoRespons>> GetUserBookings()
        {
            try
            {
                var token = HttpContext.Request.Cookies["ttt"];
                var listBookings = _userService.GetUserBookings(token).Value;
                List<BookingDtoRespons> newList = new List<BookingDtoRespons>();
                foreach (var booking in listBookings)
                {
                    var hallName = _hallService.GetNameHall(booking.HallID);
                    BookingDtoRespons bookingDto = new()
                    {
                        Start = booking.StartBooking,
                        Duration = booking.Duration,
                        HallName = hallName
                    };
                    newList.Add(bookingDto);
                }

                return Ok(newList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteBooking")]    // удалить бронь зала 
        public ActionResult DeleteBooking(Guid id)
        {
            try
            {
                _bookingService.DeleteBooking(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
