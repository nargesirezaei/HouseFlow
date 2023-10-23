using MongoDB.Bson;

namespace HouseFlowPart1.Models
{
    public class HouseTypes
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; } = "";
    }
}
