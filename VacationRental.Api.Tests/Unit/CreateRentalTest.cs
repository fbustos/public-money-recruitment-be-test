using System;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using VacationRental.Api.Handlers.RentalHandler;
using VacationRental.Api.Models.Requests;
using VacationRental.Domain.Rental;
using Xunit;

namespace VacationRental.Api.Tests.Unit
{
    public class CreateRentalTest
    {
        private const int RentalId = 1;
            
        private readonly IRentalRepository _rentalRepository;
        private readonly CreateRental _createRental;

        public CreateRentalTest()
        {
            _rentalRepository = Substitute.For<IRentalRepository>();
            _createRental = new CreateRental(_rentalRepository);
        }

        [Fact]
        public void GivenCompleteRequest_WhenCreateRental_ThenReturnsIdOfCreatedRental()
        {
            var request = new RentalBindingModel {Units = 1, PreparationTimeInDays = 1};
            var rental = new Rental(1, 1) {Id = 1};
            _rentalRepository.Add(Arg.Any<Rental>()).Returns(rental);

            var result = _createRental.Invoke(request);
            
            Assert.NotNull(result);
            Assert.Equal(rental.Id, result.Id);
        }
    }
}