using System.Collections.Generic;
using VacationRental.Domain.Rental;

namespace VacationRental.Infrastructure
{
    public class MemRentalRepository : RepositoryBase<Rental>, IRentalRepository
    {
        public MemRentalRepository(IDictionary<int, Rental> rentals) : base(rentals)
        {
        }
    }
}