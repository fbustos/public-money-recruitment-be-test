using System;
using System.Collections.Generic;

namespace VacationRental.Api.Models.Responses
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
        public List<CalendarPreparationTimeViewModel> PreparationTimes { get; set; }
    }
}
