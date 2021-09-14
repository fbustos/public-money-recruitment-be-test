using System;
using System.Linq;
using VacationRental.Api.Models.Requests;
using VacationRental.Api.Models.Responses;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rental;

namespace VacationRental.Api.Handlers.RentalHandler
{
    public class UpdateRental
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;

        public UpdateRental(IRentalRepository rentalRepository, IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }

        public ResourceIdViewModel Invoke(int rentalId, RentalBindingModel model)
        {
            var rental = _rentalRepository.Get(rentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            if (rental.Units > model.Units || rental.PreparationTimeInDays < model.PreparationTimeInDays)
            {
                _bookingRepository.List(booking => booking.RentalId == rentalId).ForEach(b =>
                {
                    ValidateUnits(model, b);
                    ValidatePreparationDays(rentalId, model, rental, b);
                });
            }

            var updatedRental = new Rental(model.Units, model.PreparationTimeInDays) {Id = rentalId};
            _rentalRepository.Update(updatedRental);
            return new ResourceIdViewModel {Id = rentalId};
        }

        private static void ValidateUnits(RentalBindingModel model, Booking b)
        {
            if (b.Unit > model.Units)
                throw new ApplicationException("There is not enough units for existing bookings");
        }
        
        private void ValidatePreparationDays(int rentalId, RentalBindingModel model, Rental rental, Booking booking)
        {
            var extraDays = model.PreparationTimeInDays - rental.PreparationTimeInDays;
            if (extraDays <= 0) return;

            for (var i = 0; i < extraDays; i++)
            {
                var date = booking.StartDate.AddDays(booking.Nights + rental.PreparationTimeInDays + i);
                var isOccupied = _bookingRepository
                    .List(book => book.RentalId == rentalId && book.StartDate == date)
                    .Any(book => book.Unit == booking.Unit);

                // TODO: Ask if the unit of a book can change. Now assume it cannot.
                if (isOccupied)
                    throw new ApplicationException(
                        "Cannot increase preparation time because overlaps with existing bookings");
            }
        }
    }
}