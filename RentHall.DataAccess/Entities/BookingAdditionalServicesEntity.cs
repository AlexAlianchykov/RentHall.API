namespace RentHall.DataAccess.Entities
{
    public class BookingAdditionalServicesEntity
    {
        public Guid BookingID { get; set; }
        public BookingEntity BookingEntity { get; set; }

        public Guid AdditionalServicesID { get; set; }
        public AdditionalServicesEntity AdditionalServicesEntity { get; set; }
    }
}
