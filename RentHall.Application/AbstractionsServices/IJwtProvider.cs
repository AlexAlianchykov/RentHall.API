using RentHall.Core.Models.AggregateUser;

namespace RentHall.Application.AbstractionsServices
{
    public interface IJwtProvider
    {
        public string GenerateToken(User user);
    }
}
