using System;
using VacationRental.Api.Models.Responses;
using VacationRental.Domain.Bookings;

namespace VacationRental.Api.Handlers.BookingHandler
{
    public class GetBooking
    {
        private readonly IBookingRepository _bookingRepository;

        public GetBooking(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public BookingViewModel Invoke(int id)
        {
            var booking = _bookingRepository.Get(id);
            if (booking == null)
                throw new ApplicationException("Booking not found");

            return new BookingViewModel
            {
                Id = booking.Id,
                RentalId = booking.RentalId,
                Nights = booking.Nights,
                Start = booking.StartDate
            };
        }
    }
}