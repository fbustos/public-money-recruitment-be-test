using System;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using VacationRental.Api.Handlers.BookingHandler;
using VacationRental.Domain.Bookings;
using Xunit;

namespace VacationRental.Api.Tests.Unit
{
    public class GetBookingTest
    {
        private const int RentalId = 1;
        private const int BookingId = 1;
        
        private readonly IBookingRepository _bookingRepository;
        private readonly GetBooking _getBooking;

        public GetBookingTest()
        {
            _bookingRepository = Substitute.For<IBookingRepository>();
            _getBooking = new GetBooking(_bookingRepository);
        }

        [Fact]
        public void GivenBookingId_WhenGetBookingById_ThenAGetReturnsTheBooking()
        {
            var booking = new Booking(RentalId, DateTime.Now, 2, 1) {Id = BookingId};
            _bookingRepository.Get(BookingId).Returns(booking);

            var result = _getBooking.Invoke(BookingId);

            Assert.NotNull(result);
            Assert.Equal(booking.Id, result.Id);
            Assert.Equal(booking.RentalId, result.RentalId);
            Assert.Equal(booking.Nights, result.Nights);
        }

        [Fact]
        public void GivenBookingId_WhenGetBookingById_ThenThrowApplicationExceptionIfBookingNotExists()
        {
            _bookingRepository.Get(BookingId).ReturnsNull();

            Assert.Throws<ApplicationException>(() => _getBooking.Invoke(BookingId));
        }
    }
}