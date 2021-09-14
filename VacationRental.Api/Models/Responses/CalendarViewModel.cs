using System.Collections.Generic;

namespace VacationRental.Api.Models.Responses
{
    public class CalendarViewModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateViewModel> Dates { get; set; }
    }
}
