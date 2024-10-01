using System.ComponentModel.DataAnnotations;

namespace RentHall.API.Contracts
{
    public class BookingDtoRequest
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public Guid HallId { get; set; }
        public List<string>? services { get; set; }
    }
}
