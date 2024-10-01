using System.ComponentModel.DataAnnotations;

namespace RentHall.API.Contracts
{
    public class GetFreeHallsDto
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }
        [Required]
        public int Duration { get; set; }
        public int Capacity { get; set; }
    }
}
