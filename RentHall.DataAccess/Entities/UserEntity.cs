namespace RentHall.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string EmailUser { get; set; }
        public string HashPassword { get; set; }
        public string Role { get; set; }
        public DateTime CreateDate { get; set; }

        public List<BookingEntity> Bookings { get; set; }
    }
}
