using HouseFlowPart1.Models;

namespace HouseFlowPart1
{
    public class HouseDetailViewModel
    {
        public HousesDTO House { get; set; } = new HousesDTO();
        public List<HouseImages> Images { get; set; } = new List<HouseImages> { };

        public RentedHauses RentedHauses { get; set; } = new RentedHauses();
    }
}
