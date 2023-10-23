using MongoDB.Bson;

namespace HouseFlowPart1.Models
{
    public class Houses
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; } = "";
        public string Address { get; set; } = "";
        public int Price { get; set; }
        public ObjectId TypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public ObjectId OwnerId { get; set; }
        public string Description { get; set; } = "";
    }
}
