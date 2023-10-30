using HouseFlowPart1.Models;

namespace HouseFlowPart1
{
    public class HouseDetailViewModel
    {
        // Represents details about a house.
        public HousesDTO House { get; set; } = new HousesDTO();
        // Contains a list of images related to the house.
        public List<HouseImages> Images { get; set; } = new List<HouseImages> { };
        // Represents details about a rented house, if applicable.
        public RentedHauses RentedHauses { get; set; } = new RentedHauses();
    }
}
