using CSharpFunctionalExtensions;

namespace RentHall.Core.Models.AggregateHall
{
    public class Booking
    {
        private Booking(
            Guid id,
            DateTime start,
            int duration,
            DateTime dateTime,
            Guid hallid,
            Guid userid)
        {
            Id = id;
            StartBooking = start;
            Duration = duration;
            CreateDate = dateTime;
            HallID = hallid;
            UserID = userid;
        }

        public Guid Id { get; }
        public DateTime StartBooking { get; }
        public int Duration { get; }
        public DateTime CreateDate { get; }
        public Guid HallID { get; }
        public Guid UserID { get; }

        public static Result<Booking> Create(
            Guid id,
            DateTime start,
            int duration,
            DateTime dateTime,
            Guid hallid,
            Guid userid)
        {
            if (duration < 0 || duration > 17)
            {
                return Result.Failure<Booking>("The duration is indicated incorrectly");
            }

            DateTime now = DateTime.Now;
            if (start < now || start > now.AddDays(61) )
            {
                return Result.Failure<Booking>("The booking start date is incorrect");
            }

            if (start.Hour < 6 || (start.Hour + duration) > 23 || start.Hour > 22)
            {
                return Result.Failure<Booking>("Incorrect booking start time");
            }

            return Result.Success(new Booking(id, start, duration, dateTime, hallid, userid));
        } 
    }
}
