using System.ComponentModel.DataAnnotations;

namespace RentHall.API.Contracts
{
    public class CreateUserDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } 
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
