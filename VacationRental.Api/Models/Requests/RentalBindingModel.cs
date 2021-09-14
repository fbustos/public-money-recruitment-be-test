using System.ComponentModel.DataAnnotations;

namespace VacationRental.Api.Models.Requests
{
    public class RentalBindingModel
    {
        [Range(1, int.MaxValue)]
        public int Units { get; set; }
        
        [Range(0, int.MaxValue)]
        public int PreparationTimeInDays { get; set; }
    }
}
