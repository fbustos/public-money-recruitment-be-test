using System;
using VacationRental.Api.Models.Responses;
using VacationRental.Domain.Rental;

namespace VacationRental.Api.Handlers.RentalHandler
{
    public class GetRental
    {
        private readonly IRentalRepository _rentalRepository;

        public GetRental(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public RentalViewModel Invoke(int id)
        {
            var rental = _rentalRepository.Get(id);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            return new RentalViewModel
            {
                Id = rental.Id,
                Units = rental.Units,
                PreparationTimeInDays = rental.PreparationTimeInDays
            };
        }
    }
}