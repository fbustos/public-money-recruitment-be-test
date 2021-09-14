using System.Collections.Generic;
using VacationRental.Domain.Bookings;

namespace VacationRental.Infrastructure
{
    public class MemBookingRepository : RepositoryBase<Booking>, IBookingRepository
    {
        public MemBookingRepository(IDictionary<int, Booking> bookings) : base(bookings)
        {
        }
    }
}