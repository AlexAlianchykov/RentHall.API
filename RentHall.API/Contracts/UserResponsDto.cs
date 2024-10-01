using RentHall.Core.Models.AggregateUser;

namespace RentHall.API.Contracts
{
    public class UserResponsDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        
    }
}
