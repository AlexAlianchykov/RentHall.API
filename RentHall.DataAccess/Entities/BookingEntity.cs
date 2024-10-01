namespace RentHall.DataAccess.Entities
{
    public class BookingEntity
    {
        public Guid Id { get; set; }
        public DateTime StartBooking { get; set; }
        public int Duration { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime CreateDate { get; set; }

        public Guid HallID { get; set; }
        public HallEntity HallEntity { get; set; }
        public Guid UserID { get; set; }
        public UserEntity UserEntity { get; set; }
        public List<BookingAdditionalServicesEntity> BookingsServces { get; set; }
    }
}
