using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Handlers;
using VacationRental.Api.Handlers.BookingHandler;
using VacationRental.Api.Models.Requests;
using VacationRental.Api.Models.Responses;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly GetBooking _getBooking;
        private readonly CreateBooking _createBooking;

        public BookingsController(GetBooking getBooking, CreateBooking createBooking)
        {
            _getBooking = getBooking;
            _createBooking = createBooking;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            return _getBooking.Invoke(bookingId);
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            return _createBooking.Invoke(model);
        }
    }
}
