using MongoDB.Bson;

namespace HouseFlowPart1.Models
{
    public class Houses
    {
        // Unique identifier for the house
        public ObjectId Id { get; set; }
        public string Title { get; set; } = "";
        public string Address { get; set; } = "";
        public int Price { get; set; }
        // Identifier of the house type
        public ObjectId TypeId { get; set; }
        // Start date for availability
        public DateTime FromDate { get; set; }
        // End date for availability
        public DateTime ToDate { get; set; }
        // Identifier of the owner of the house
        public ObjectId OwnerId { get; set; }
        public string Description { get; set; } = "";
    }
}
