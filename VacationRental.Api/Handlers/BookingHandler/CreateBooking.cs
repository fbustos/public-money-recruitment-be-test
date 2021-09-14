using System;
using System.Linq;
using VacationRental.Api.Models.Requests;
using VacationRental.Api.Models.Responses;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rental;

namespace VacationRental.Api.Handlers.BookingHandler
{
    public class CreateBooking
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;

        public CreateBooking(IBookingRepository bookingRepository, IRentalRepository rentalRepository)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
        }

        public ResourceIdViewModel Invoke(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nights must be positive");

            var rental = _rentalRepository.Get(model.RentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            var availableUnit = FindAvailableUnit(model, rental);
            var booking = _bookingRepository.Add(new Booking(rental.Id, model.Start, model.Nights, availableUnit));
            return new ResourceIdViewModel {Id = booking.Id};
        }

        private int FindAvailableUnit(BookingBindingModel model, Rental rental)
        {
            const int firstUnit = 1;
            var bookings = _bookingRepository.List(b =>
                b.RentalId == model.RentalId
                && OverlapsBooking(b, model, rental.PreparationTimeInDays)
            );

            if (bookings == null || !bookings.Any())
                return firstUnit;

            for (var unit = firstUnit; unit <= rental.Units; unit++)
            {
                if (bookings.All(b => b.Unit != unit))
                {
                    return unit;
                }
            }

            throw new ApplicationException("Not available");
        }

        private bool OverlapsBooking(Booking booking, BookingBindingModel model, int preparationTime)
        {
            var startDateIsInTheMiddleOfBooking = booking.StartDate <= model.Start.Date &&
                                                  booking.StartDate.AddDays(booking.Nights + preparationTime) >
                                                  model.Start.Date;
            var endDateIsInTheMiddleOfBooking =
                booking.StartDate < model.Start.AddDays(model.Nights + preparationTime) &&
                booking.StartDate.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights);

            var hasBookingInTheMiddle = booking.StartDate > model.Start &&
                                        booking.StartDate.AddDays(booking.Nights) < model.Start.AddDays(model.Nights);

            return startDateIsInTheMiddleOfBooking
                   || endDateIsInTheMiddleOfBooking
                   || hasBookingInTheMiddle;
        }
    }
}