using HouseFlowPart1.Models;

namespace HouseFlowPart1
{
    public class HousesEditDTO
    {
        public Houses Data { get; set; } = new Houses();
        public List<HouseTypes> Types { get; set; } = new List<HouseTypes> { };
    }
}
