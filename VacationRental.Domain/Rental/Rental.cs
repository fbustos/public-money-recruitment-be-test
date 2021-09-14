namespace VacationRental.Domain.Rental
{
    public class Rental : BaseEntity
    {
        public Rental(int units, int preparationTimeInDays)
        {
            Units = units;
            PreparationTimeInDays = preparationTimeInDays;
        }
        
        public int Units { get; }
        public int PreparationTimeInDays { get; }
    }
}