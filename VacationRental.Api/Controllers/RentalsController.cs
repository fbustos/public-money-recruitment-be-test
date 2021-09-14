using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Handlers.RentalHandler;
using VacationRental.Api.Models.Requests;
using VacationRental.Api.Models.Responses;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly GetRental _getRental;
        private readonly CreateRental _createRental;
        private readonly UpdateRental _updateRental;

        public RentalsController(GetRental getRental, CreateRental createRental, UpdateRental updateRental)
        {
            _getRental = getRental;
            _createRental = createRental;
            _updateRental = updateRental;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            return _getRental.Invoke(rentalId);
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            return _createRental.Invoke(model);
        }

        [HttpPut]
        [Route("{rentalId:int}")]
        public ResourceIdViewModel Post(int rentalId, RentalBindingModel model)
        {
            return _updateRental.Invoke(rentalId, model);
        }
    }
}