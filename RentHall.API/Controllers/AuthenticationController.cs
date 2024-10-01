using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentHall.API.Contracts;
using RentHall.Application.AbstractionsServices;
using RentHall.Application.Services;

namespace RentHall.API.Controllers
{
    [Route("api/AuthenticationController")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IUserService _userService;
        private AuthenticationService _authenticationService;
        public AuthenticationController(
            IUserService userService,
            AuthenticationService service)
        {
            _userService = userService;
            _authenticationService = service;
        }


        [HttpPost("Authentication")]  
        public ActionResult<Guid> CreateNewUser([FromBody] CreateUserDto dto)
        {
            try
            {
                return Ok(_userService.CreateUser(dto.Name, dto.Email, dto.Password).Value); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            try
            {
                var token = _authenticationService.Login(dto.Email, dto.Password).Value;
                HttpContext.Response.Cookies.Append("ttt", token);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
