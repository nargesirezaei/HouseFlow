using HouseFlowPart1.Models;

namespace HouseFlowPart1
{
    public class HousesEditDTO
    {
        // Represents the data of a house, typically used for editing.
        public Houses Data { get; set; } = new Houses();
        // A list of available house types, which can be associated with the house.
        public List<HouseTypes> Types { get; set; } = new List<HouseTypes> { };
    }
}
