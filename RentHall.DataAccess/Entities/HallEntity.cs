namespace RentHall.DataAccess.Entities
{
    public class HallEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Сapacity { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime CreateDate { get; set; }

        
        public List<BookingEntity>? Bookings { get; set; }
        public List<HallAdditionalServicesEntity> HallsServices { get; set; }
    }
}
