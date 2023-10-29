using HouseFlowPart1.Models;

namespace HouseFlowPart1
{
    public class HouseImagesViewModel
    {
        // Represents a House object associated with the images.
        public HousesDTO House { get; set; } = new HousesDTO();
        // Represents a single image associated with the house.
        public HouseImages Image { get; set; } = new HouseImages();
    }
}
