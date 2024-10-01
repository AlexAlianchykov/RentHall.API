using CSharpFunctionalExtensions;
using RentHall.Core.Models.AggregateHall;
using RentHall.Core.Models.AggregateUser;

namespace RentHall.Core.AbstractionsRepositories.UserAbstractions
{
    public interface IUserRepository
    {
        public Result<List<User>> GetUsers();
        public Result<User> GetUser(Email email);
        public Result<List<Booking>> GetBookings(Guid id);
        public Result<Guid> Create(User user);
        public Result Delete(Email email);

    }
}
