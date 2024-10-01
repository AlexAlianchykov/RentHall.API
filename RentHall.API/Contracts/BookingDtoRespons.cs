using System.ComponentModel.DataAnnotations;

namespace RentHall.API.Contracts
{
    public class BookingDtoRespons
    {
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public string HallName { get; set; }
    }
}
