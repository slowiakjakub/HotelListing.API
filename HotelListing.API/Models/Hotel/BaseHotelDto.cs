using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Hotel
{
    public abstract class BaseHotelDto
    {

        [Required]
        public string Name { get; set; }
        public string Address { get; set; }

    }
}
