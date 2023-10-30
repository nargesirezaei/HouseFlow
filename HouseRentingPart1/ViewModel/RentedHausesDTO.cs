namespace HouseFlowPart1.Models
{
    public class RentedHausesDTO
    {
        public string Id { get; set; } = "";
        public string UserId { get; set; } = "";
        public string HauseId { get; set; } = "";
        // Details of the rented house, including its attributes.
        public HousesDTO House { get; set; } = new HousesDTO();
        // Information about the house's images.
        public HouseImages HouseImage { get; set; } = new HouseImages();
        public string FromDate { get; set; } = "";
        public string ToDate { get; set; } = "";
        public int Numbers { get; set; }
        public int TotalPrice { get; set; }
        // Date when the rental request was made.
        public DateTime RequestDate { get; set; } = DateTime.Now;
    }
}
