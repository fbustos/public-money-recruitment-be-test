using System;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using VacationRental.Api.Handlers.RentalHandler;
using VacationRental.Domain.Rental;
using Xunit;

namespace VacationRental.Api.Tests.Unit
{
    public class GetRentalTest
    {
        private const int RentalId = 1;
            
        private readonly IRentalRepository _rentalRepository;
        private readonly GetRental _getRental;

        public GetRentalTest()
        {
            _rentalRepository = Substitute.For<IRentalRepository>();
            _getRental = new GetRental(_rentalRepository);
        }

        [Fact]
        public void GivenRentalId_WhenGetRental_ThenAGetReturnsTheRental()
        {
            var rental = new Rental(2, 3) {Id = RentalId};
            _rentalRepository.Get(RentalId).Returns(rental);

            var result = _getRental.Invoke(RentalId);
            
            Assert.NotNull(result);
            Assert.Equal(rental.Id, result.Id);
            Assert.Equal(rental.Units, result.Units);
            Assert.Equal(rental.PreparationTimeInDays, result.PreparationTimeInDays);
        }
        
        [Fact]
        public void GivenRentalId_WhenGetRental_ThenThrowApplicationExceptionIfRentalNotExists()
        {
            _rentalRepository.Get(RentalId).ReturnsNull();

            Assert.Throws<ApplicationException>(() => _getRental.Invoke(RentalId));
        }
    }
}