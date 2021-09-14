using VacationRental.Api.Models.Requests;
using VacationRental.Api.Models.Responses;
using VacationRental.Domain.Rental;

namespace VacationRental.Api.Handlers.RentalHandler
{
    public class CreateRental
    {
        private readonly IRentalRepository _rentalRepository;

        public CreateRental(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public ResourceIdViewModel Invoke(RentalBindingModel model)
        {
            // TODO: Validate 'model' fields
            
            var rental = _rentalRepository.Add(new Rental(model.Units, model.PreparationTimeInDays));
            return new ResourceIdViewModel
            {
                Id = rental.Id
            };
        }
    }
}