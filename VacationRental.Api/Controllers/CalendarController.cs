using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Handlers.CalendarHandler;
using VacationRental.Api.Models.Responses;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly GetCalendar _getCalendar;

        public CalendarController(GetCalendar getCalendar)
        {
            _getCalendar = getCalendar;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            return _getCalendar.Invoke(rentalId, start, nights);
        }
    }
}
