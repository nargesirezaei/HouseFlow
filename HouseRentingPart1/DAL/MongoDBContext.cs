using MongoDB.Driver;

namespace HouseFlowPart1.Models
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IConfiguration configuration)
        {
            var mongoConnectionString = configuration["MongoDBSettings:ConnectionString"];
            var databaseName = configuration["MongoDBSettings:DatabaseName"];

            var client = new MongoClient(mongoConnectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
