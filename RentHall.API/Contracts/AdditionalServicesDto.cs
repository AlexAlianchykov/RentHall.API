using System.ComponentModel.DataAnnotations;

namespace RentHall.API.Contracts
{
    public class AdditionalServicesDto
    {
        [Required]
        [MaxLength(15)]
        public string Name { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Currency { get; set; }
    }
}
