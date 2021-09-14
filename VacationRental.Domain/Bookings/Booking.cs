using System;

namespace VacationRental.Domain.Bookings
{
    public class Booking : BaseEntity
    {
        public Booking(int rentalId, DateTime startDate, int nights, int unit)
        {
            RentalId = rentalId;
            StartDate = startDate.Date;
            Nights = nights;
            Unit = unit;
        }

        public int RentalId { get; }
        public DateTime StartDate { get; }
        public int Nights { get; }
        public int Unit { get; }
    }
}