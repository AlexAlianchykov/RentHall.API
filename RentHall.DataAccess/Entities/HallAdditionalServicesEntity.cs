namespace RentHall.DataAccess.Entities
{
    public class HallAdditionalServicesEntity
    {
        public Guid HallID { get; set; }
        public HallEntity HallEntity { get; set; }

        public Guid AdditionalServicesID { get; set; }
        public AdditionalServicesEntity AdditionalServicesEntity { get; set; }
    }
}
