using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Api.Models.Responses;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rental;

namespace VacationRental.Api.Handlers.CalendarHandler
{
    public class GetCalendar
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;

        public GetCalendar(IRentalRepository rentalRepository, IBookingRepository bookingRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }

        public CalendarViewModel Invoke(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");

            var rental = _rentalRepository.Get(rentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            var result = new CalendarViewModel 
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>() 
            };
            for (var i = 0; i < nights; i++)
            {
                var date = start.Date.AddDays(i);
                var calendarDate = new CalendarDateViewModel
                {
                    Date = date,
                    Bookings = GetExistingBookings(rentalId, date),
                    PreparationTimes = GetBookingsInPreparationTimes(rentalId, date, rental.PreparationTimeInDays)
                };

                result.Dates.Add(calendarDate);
            }

            return result;
        }
        
        // TODO: Pending extract this methods to BookingService
        private List<CalendarPreparationTimeViewModel> GetBookingsInPreparationTimes(int rentalId, DateTime date, int preparationTimeInDays)
        {
            return _bookingRepository
                .List(b => b.RentalId == rentalId && IsOverlappingBookingPreparationTime(b, date, preparationTimeInDays))
                .Select(b =>new CalendarPreparationTimeViewModel { Unit = b.Unit }).ToList();
        }

        private List<CalendarBookingViewModel> GetExistingBookings(int rentalId, DateTime date)
        {
            return _bookingRepository
                .List(b => b.RentalId == rentalId && IsOverlappingBooking(b, date))
                .Select(b => new CalendarBookingViewModel { Id = b.Id, Unit = b.Unit } ).ToList();
        }

        private bool IsOverlappingBooking(Booking b, DateTime date)
        {
            return b.StartDate <= date && b.StartDate.AddDays(b.Nights) > date;
        }

        private bool IsOverlappingBookingPreparationTime(Booking b, DateTime date, int preparationTimeInDays)
        {
            return b.StartDate.AddDays(b.Nights) <= date && b.StartDate.AddDays(b.Nights + preparationTimeInDays) > date;
        }
    }
}