using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentHall.API.Contracts;
using RentHall.Application.AbstractionsServices;
using RentHall.Application.Services;
using RentHall.Core.Models.AggregateHall;
using RentHall.Core.Models.AggregateUser;
using System.Collections.Generic;

namespace RentHall.API.Controllers
{
    [Authorize(Policy = "AdminPolisy")]
    [Route("api/AdminController")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IUserService _userService;
        private IAdditionalServicesServices _additionalServicesServices;
        private IHallService _hallService;
        public AdminController(
            IUserService userService,
            IAdditionalServicesServices additionalServicesServices,
            IHallService hallService)
        {
            _userService = userService;
            _additionalServicesServices = additionalServicesServices;
            _hallService = hallService;
        }

        [HttpGet("GetAllUsers")] // список всех юзеров
        public ActionResult<List<UserResponsDto>> GetAllUsers()
        {
            try
            {
                var users = _userService.GetAllUsers().Value;
                List<UserResponsDto> newList = new List<UserResponsDto>();
                foreach (var user in users)
                {
                    UserResponsDto userResponsDto = new()
                    {
                        Name = user.Name,
                        Email = user.Email.EmailUser,
                        Role = user.Role
                    };
                    newList.Add(userResponsDto);
                }
                return Ok(newList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteUser")] // удалить определённого юзера по email 
        public ActionResult DeleteUser(string email)
        {
            try
            {
                _userService.DeleteUser(email);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAllServices")]  // список всех дополнительных услуг 
        public ActionResult<List<AdditionalServicesDto>> GetAllServices()
        {
            try
            {
                var listAdditServ = _additionalServicesServices.GetAllServices().Value;
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

        [HttpPost("CreateNewService")] // добавить новую дополнительную услугу 
        public ActionResult<Guid> CreateNewService([FromBody] AdditionalServicesDto dto)
        {
            try
            {
               return Ok(_additionalServicesServices.CreateAdditionalServices(dto.Name, dto.Amount, dto.Currency).Value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("UpdateService")] // обновить дополнительную услугу 
        public ActionResult UpdateService(Guid id, [FromBody] AdditionalServicesDto dto)
        {
            try
            {
                _additionalServicesServices.UpdateAdditionalServices(id, dto.Name, dto.Amount, dto.Currency);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteService")]   // удалить дополнительную услугу
        public ActionResult DeleteService(Guid id)
        {
            try
            {
                _additionalServicesServices.DeleteAdditionalServices(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAllHalls")]  // список всех залов
        public ActionResult<List<HallDtoRequest>> GetAllHalls()
        {
            try
            {
                var listHall = _hallService.GetAllHall().Value;
                List<HallDtoRequest> newList = new List<HallDtoRequest>();
                foreach (var hall in listHall)
                {
                    var listServices = _additionalServicesServices.GetInHallServices(hall.Name).Value;
                    List<string> nameServise = new List<string>();
                    foreach (var service in listServices)
                    {
                        nameServise.Add(service.Name);
                    }
                    HallDtoRequest newHall = new()
                    {
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

        [HttpPost("CreateNewHall")]  // добавть новый зал
        public ActionResult<Guid> CreateNewHall([FromBody] HallDtoRequest dto)
        {
            try
            {
                return Ok(_hallService.CreateHall(dto.Name, dto.Capacity, dto.Amount, dto.Currency, dto.additionalServices).Value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("UpdateHall")]   // обновить информацию о зале
        public ActionResult UpdateHall([FromBody] UpdateHallDto dto)
        {
            try
            {
                _hallService.UpdateHall(
                    dto.HallId,
                    dto.Name,
                    dto.Capacity,
                    dto.Amount,
                    dto.Currency,
                    dto.additionalServices,
                    dto.removeServices);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteHall")]    // удалить зал 
        public ActionResult DeleteHall(Guid id)
        {
            try
            {
                _hallService.DeleteHall(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
