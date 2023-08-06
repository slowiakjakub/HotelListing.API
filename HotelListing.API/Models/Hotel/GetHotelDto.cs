namespace HotelListing.API.Models.Hotel
{
    public class GetHotelDto : BaseHotelDto
    {
        public int Id { get; set; }
        public double Rating { get; set; }
    }
 }
