namespace RentHall.DataAccess.Entities
{
    public class AdditionalServicesEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime CreateDate { get; set; }

        public List<BookingAdditionalServicesEntity> BookingsServces { get; set; }
        public List<HallAdditionalServicesEntity> HallsServices { get; set; }
    }
}
