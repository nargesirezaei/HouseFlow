using MongoDB.Bson;
namespace HouseFlowPart1
{
	public class HousesDTO
	{
        public ObjectId Id { get; set; }
        public string Title { get; set; } = "";

        public string Address { get; set; } = "";
        public int Price { get; set; } = 0;
        public string TypeId { get; set; } = "";
        public string FromDate { get; set; } = "";
        public string ToDate { get; set; } = "";
        public string OwnerId { get; set; } = "";
        public string Description { get; set; } = "";
        public string? TypeTitle { get; set; } = "";
    }
}

