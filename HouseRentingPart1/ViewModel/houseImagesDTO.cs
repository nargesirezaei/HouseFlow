using HouseFlowPart1.Models;

namespace HouseFlowPart1
{
    public class HouseImagesDTO
    {
        // Represents a list of house images.
        public List<HouseImages> Images { get; set; } = new List<HouseImages>();
        // Represents the ID of the associated house.
        public string HouseId { get; set; } = "";
    }
}
