using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentHall.Application.AbstractionsServices;
using RentHall.Application.Services.Jwt;
using RentHall.Core.AbstractionsRepositories.HallAbstractions;
using RentHall.Core.Models.AggregateHall;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RentHall.Application.Services
{
    public class BookingService: IBookingService
    {
        private IBookingRepository _bookingRepository;
        private IConfiguration _configuration;
        private JwtOptions _jwtOptions;
        public BookingService(
            IBookingRepository bookingRepository, 
            IConfiguration configuration,
            IOptions<JwtOptions> options)
        {
            _bookingRepository = bookingRepository;
            _configuration = configuration;
            _jwtOptions = options.Value;
        }

        public Result<List<Hall>> GetFreeHalls(
            DateTime start,
            int duration,
            int capacity)
        {
            try
            {
                return Result.Success(_bookingRepository.GetFree(start, duration, capacity).Value);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Hall>>(ex.Message);
            }
        }

        public Result<decimal> CreateBookingHall(
            string token,
            DateTime start,
            int duration,
            Guid hallId,
            List<string>? services)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                // Валидация токена
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey))
                };

                // Попытка валидации токена и получения клеймов
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                
                var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "UserId");

                var userId = Guid.Parse(userIdClaim.Value);

                DateTime bookingDate = new DateTime(
                        start.Year,
                        start.Month,
                        start.Day,
                        start.Hour,
                        0, 0, 0);


                var newBooking = Booking.Create(Guid.NewGuid(), bookingDate, duration, DateTime.Now, hallId, userId).Value;
                return Result.Success(_bookingRepository.Create(newBooking, services).Value);
            }
            catch (Exception ex)
            {
                return Result.Failure<decimal>(ex.Message);
            }
        }

        public Result DeleteBooking(Guid id)
        {
            try
            {
                _bookingRepository.Delete(id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
