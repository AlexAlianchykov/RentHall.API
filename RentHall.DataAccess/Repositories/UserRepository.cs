using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using RentHall.Core.AbstractionsRepositories.UserAbstractions;
using RentHall.Core.Models.AggregateHall;
using RentHall.Core.Models.AggregateUser;
using RentHall.DataAccess.Entities;

namespace RentHall.DataAccess.Repositories
{
    public class UserRepository: IUserRepository
    {
        private ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public Result<List<User>> GetUsers()
        {
            var allUsers = _dbContext.Users.AsNoTracking().ToList();

            List<User> users = new List<User>();

            foreach(var user in allUsers)
            {
                var newEmail = Email.Create(user.EmailUser).Value;

                var newUser = User.Create(
                    user.Id,
                    user.Name,
                    newEmail,
                    user.HashPassword,
                    user.Role,
                    user.CreateDate).Value;
                users.Add(newUser);
            }

            return Result.Success(users);
        }

        public Result<User> GetUser(Email email)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.EmailUser == email.EmailUser);
            if (user == null)
            {
                return Result.Failure<User>($"User with {email.EmailUser.ToUpper()} does not exist");
            }

            var newUser = User.Create(
                user.Id,
                user.Name,
                email,
                user.HashPassword,
                user.Role,
                user.CreateDate).Value;

            return Result.Success(newUser);
        }

        public Result<List<Booking>> GetBookings(Guid id)
        {
            var userBookings = _dbContext.Bookings
            .Where(b => b.UserID == id)  
            .ToList();

            List<Booking> newList = new List<Booking>();
            foreach (var booking in userBookings)
            {
                var newBooking = Booking.Create(
                    booking.Id,
                    booking.StartBooking,
                    booking.Duration,
                    booking.CreateDate,
                    booking.HallID,
                    booking.UserID).Value;
                newList.Add(newBooking);
            }
            return Result.Success(newList);
        }

        public Result<Guid> Create(User user)
        {
            var copyEmailUser = _dbContext.Users.FirstOrDefault(x => x.EmailUser == user.Email.EmailUser);

            if (copyEmailUser != null)
            {
                return Result.Failure<Guid>("A user with this email already exists");
            }

            UserEntity userEntity = new()
            {
                Id = user.Id,
                Name = user.Name,
                EmailUser = user.Email.EmailUser,
                HashPassword = user.HashPassword,
                Role = user.Role,
                CreateDate = user.CreateDate
            };

            _dbContext.Users.Add(userEntity);
            _dbContext.SaveChanges();

            return Result.Success(userEntity.Id);
        } 

        public Result Delete(Email email)
        {
            var user = _dbContext.Users.FirstOrDefault(a => a.EmailUser == email.EmailUser);

            if (user == null)
            {
                return Result.Failure($"User with {email.EmailUser.ToUpper()} does not exist");
            }

            _dbContext.Users
                .Where(x => x.EmailUser == email.EmailUser)
                .ExecuteDelete();

            var bookings = _dbContext.Bookings
                                .Where(y => y.UserID == user.Id)
                                .ToList();

            foreach (var booking in bookings)
            {
                _dbContext.BookingsAdditionals
                    .Where(f => f.BookingID == booking.Id)
                    .ExecuteDelete();
            }

            _dbContext.Bookings
                .Where(y => y.UserID == user.Id)
                .ExecuteDelete();

            return Result.Success();
        } 
    }
}
