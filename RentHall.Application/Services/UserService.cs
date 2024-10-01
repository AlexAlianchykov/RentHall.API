using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentHall.Application.AbstractionsServices;
using RentHall.Application.Services.Jwt;
using RentHall.Core.AbstractionsRepositories.UserAbstractions;
using RentHall.Core.Models.AggregateHall;
using RentHall.Core.Models.AggregateUser;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RentHall.Application.Services
{
    public class UserService: IUserService
    {
        private IUserRepository _userRepository;
        private IPasswordHash _passwordHash;
        private JwtOptions _jwtOptions;
        public UserService(IUserRepository userRepository,
            IPasswordHash passwordHash,
            IOptions<JwtOptions> options)
        {
            _userRepository = userRepository;
            _passwordHash = passwordHash;
            _jwtOptions = options.Value;
        }

        public Result<Guid> CreateUser(
            string name,
            string email,
            string password)
        {
            try
            {
                var hashPassword = _passwordHash.Generate(password);
                var newEmail = Email.Create(email).Value;
                var newUser = User.Create(Guid.NewGuid(), name, newEmail, hashPassword, "user", DateTime.Now).Value;
                return Result.Success(_userRepository.Create(newUser).Value);
            }
            catch(Exception ex)
            {
                return Result.Failure<Guid>(ex.Message);
            }
        }


        public Result<List<User>> GetAllUsers()
        {
            try
            {
                return Result.Success(_userRepository.GetUsers().Value);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<User>>(ex.Message);
            }
        }

        public Result<User> GetUser(string email)
        {
            try
            {
                var newEmail = Email.Create(email).Value;
                return Result.Success(_userRepository.GetUser(newEmail).Value);
            }
            catch (Exception ex)
            {
                return Result.Failure<User>(ex.Message);
            }
        }

        public Result<List<Booking>> GetUserBookings(string token)
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

                return Result.Success(_userRepository.GetBookings(userId).Value);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Booking>>(ex.Message);
            }
        }

        public Result DeleteUser(string email)
        {
            try
            {
                var newEmail = Email.Create(email).Value;
                _userRepository.Delete(newEmail);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
