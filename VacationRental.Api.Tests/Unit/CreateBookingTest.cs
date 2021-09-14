using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NSubstitute;
using VacationRental.Api.Handlers.BookingHandler;
using VacationRental.Api.Models.Requests;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rental;
using Xunit;

namespace VacationRental.Api.Tests.Unit
{
    public class CreateBookingTest
    {
        private const int RentalId = 1;

        private static readonly Rental Rental = new Rental(2, 1) {Id = RentalId};
        
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly CreateBooking _createBooking;

        public CreateBookingTest()
        {
            _bookingRepository = Substitute.For<IBookingRepository>();
            _rentalRepository = Substitute.For<IRentalRepository>();
            _createBooking = new CreateBooking(_bookingRepository, _rentalRepository);
        }

        [Fact]
        public void GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var bookingRequest = GivenBookingRequest();
            var expectedBooking = GivenExpectedBooking();
            
            WhenGetRentalById();
            WhenFindOverlappingBookings();
            WhenSavesCreatedBooking(expectedBooking);
            
            var result = _createBooking.Invoke(bookingRequest);
            
            Assert.NotNull(result);
            Assert.Equal(expectedBooking.Id, result.Id);
        }
        
        [Fact]
        public void GivenCompleteRequest_WhenPostBooking_ThenThrowsExceptionBecauseNotAvailable()
        {
            var bookingRequest = GivenBookingRequest();
            
            WhenGetRentalById();
            WhenFindOverlappingBookingsUnavailable();

            var exception = Assert.Throws<ApplicationException>(() => { _createBooking.Invoke(bookingRequest); });

            Assert.Equal("Not available", exception.Message);
            _bookingRepository.DidNotReceiveWithAnyArgs().Add(Arg.Any<Booking>());
        }

        private Booking GivenExpectedBooking()
        {
            return new Booking(RentalId, new DateTime(2000, 01, 02), 2, 1) {Id = 3};
        }

        private BookingBindingModel GivenBookingRequest()
        {
            var bookingRequest = new BookingBindingModel
            {
                Nights = 2,
                RentalId = RentalId,
                Start = new DateTime(2000, 01, 02)
            };
            return bookingRequest;
        }

        private void WhenSavesCreatedBooking(Booking expectedBooking)
        {
            _bookingRepository.Add(Arg.Any<Booking>()).ReturnsForAnyArgs(expectedBooking);
        }

        private void WhenFindOverlappingBookings()
        {
            _bookingRepository.List(Arg.Any<Expression<Func<Booking, bool>>>()).ReturnsForAnyArgs(new List<Booking>
            {
                new Booking(RentalId, new DateTime(2000, 01, 01), 1, 1),
            });
        }
        
        private void WhenFindOverlappingBookingsUnavailable()
        {
            _bookingRepository.List(Arg.Any<Expression<Func<Booking, bool>>>()).ReturnsForAnyArgs(new List<Booking>
            {
                new Booking(RentalId, new DateTime(2000, 01, 01), 1, 1),
                new Booking(RentalId, new DateTime(2000, 01, 02), 1, 2),
            });
        }

        private void WhenGetRentalById()
        {
            _rentalRepository.Get(RentalId).Returns(Rental);
        }
    }
}