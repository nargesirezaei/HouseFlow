using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HouseFlowPart1.Models
{
    public class Users
    {
        [BsonId] // Indicates that this property is the primary identifier in MongoDB
        [BsonRepresentation(BsonType.ObjectId)]  // Specifies that the property is represented as an ObjectId in MongoDB
        public ObjectId Id { get; set; } // Unique identifier for the document in MongoDB
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public bool IsAdmin { get; set; } = false;
    }
}
