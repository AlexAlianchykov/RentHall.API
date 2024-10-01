using CSharpFunctionalExtensions;
using RentHall.Core.Models.AggregateHall;
using RentHall.Core.Models.AggregateUser;

namespace RentHall.Application.AbstractionsServices
{
    public interface IUserService
    {
        public Result<Guid> CreateUser(
            string name,
            string email,
            string hashPassword);

        public Result<List<User>> GetAllUsers();

        public Result<User> GetUser(string email);

        public Result<List<Booking>> GetUserBookings(string token);

        public Result DeleteUser(string email);
    }
}
